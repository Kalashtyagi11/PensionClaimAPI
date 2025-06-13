using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PensionClaimAPI.Models;
using PensionClaimAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PensionClaimAPI.Services
{
    public interface IClaimService
    {
        Task<Claim> CreateClaimAsync(Claim claim);
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
        Task<IEnumerable<Claim>> GetApprovedClaimsAsync();
        Task<Claim> ApproveClaimAsync(Guid id);
        Task<Claim> RejectClaimAsync(Guid id);
        Task<Claim> FinalApproveClaimAsync(Guid id);
        Task<Claim> FinalRejectClaimAsync(Guid id);
    }

    public class ClaimService : IClaimService
    {
        private readonly IEmailService _emailService;
        private readonly IEmailPreferenceService _emailPreferenceService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public ClaimService(
            IEmailService emailService,
            IEmailPreferenceService emailPreferenceService,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _emailService = emailService;
            _emailPreferenceService = emailPreferenceService;
            _configuration = configuration;
            _context = context; // Initialize DbContext
        }

        public async Task<Claim> CreateClaimAsync(Claim claim)
        {
            claim.Id = Guid.NewGuid();
            claim.Status = "PENDING";
            claim.CreatedAt = DateTime.UtcNow;

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Send emails for Claim Submission workflow step
            var claimSubmissionNotifications = await _emailPreferenceService.GetWorkflowNotificationsByStepAsync("CLAIM_SUBMISSION");

            foreach (var notification in claimSubmissionNotifications)
            {
                if (notification.SendNotification)
                {
                    string emailSubject = "";
                    string emailBody = "";
                    string recipientEmail = notification.RecipientEmail;

                    if (notification.RecipientType == "APPLICANT")
                    {
                        emailSubject = "Pension Claim Submission Confirmation";
                        emailBody = $"Dear {claim.Name},\n\nYour pension claim has been successfully submitted.\n\nClaim Details:\nEmail: {claim.Email}\nCompany: {claim.CompanyName}\nEmployment Period: {claim.EmploymentStartDate:d} to {claim.EmploymentEndDate:d}";
                        recipientEmail = claim.Email; // Send to applicant's email
                    }
                    else // For other recipients (managers, etc.)
                    {
                        emailSubject = "New Pension Claim Submitted";
                        emailBody = $"A new pension claim has been submitted by {claim.Name}.\n\nClaim Details:\nEmail: {claim.Email}\nCompany: {claim.CompanyName}\nEmployment Period: {claim.EmploymentStartDate:d} to {claim.EmploymentEndDate:d}";
                    }

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await _emailService.SendEmailAsync(
                            recipientEmail,
                            emailSubject,
                            emailBody
                        );
                    }
                }
            }

            return claim;
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetApprovedClaimsAsync()
        {
            return await _context.Claims.Where(c => c.Status == "APPROVED_BY_MANAGER").ToListAsync();
        }

        public async Task<Claim> ApproveClaimAsync(Guid id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
                throw new KeyNotFoundException($"Claim with ID {id} not found");

            claim.Status = "APPROVED_BY_MANAGER";
            claim.InitialApprovalDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Send emails for Field Officer Confirmation workflow step
            var fieldOfficerNotifications = await _emailPreferenceService.GetWorkflowNotificationsByStepAsync("FIELD_OFFICER_CONFIRMATION");

            foreach (var notification in fieldOfficerNotifications)
            {
                if (notification.SendNotification)
                {
                    string emailSubject = "";
                    string emailBody = "";
                    string recipientEmail = notification.RecipientEmail;

                    if (notification.RecipientType == "APPLICANT")
                    {
                        emailSubject = "Pension Claim Update: Manager Approved";
                        emailBody = $"Dear {claim.Name},\n\nYour pension claim has been approved by the manager and is now awaiting Field Officer Confirmation.\n\nClaim Details:\nName: {claim.Name}\nCompany: {claim.CompanyName}";
                        recipientEmail = claim.Email; // Send to applicant's email
                    }
                    else // For other recipients (field officers)
                    {
                        emailSubject = "New Claim Ready for Field Officer Confirmation";
                        emailBody = $"A claim has been approved by the manager and is ready for Field Officer Confirmation.\n\nClaim Details:\nName: {claim.Name}\nEmail: {claim.Email}\nCompany: {claim.CompanyName}";
                    }

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await _emailService.SendEmailAsync(
                            recipientEmail,
                            emailSubject,
                            emailBody
                        );
                    }
                }
            }

            return claim;
        }

        public async Task<Claim> RejectClaimAsync(Guid id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
                throw new KeyNotFoundException($"Claim with ID {id} not found");

            claim.Status = "REJECTED";
            await _context.SaveChangesAsync();

            // Send emails for Rejected workflow step
            var rejectedNotifications = await _emailPreferenceService.GetWorkflowNotificationsByStepAsync("REJECTED");

            foreach (var notification in rejectedNotifications)
            {
                if (notification.SendNotification)
                {
                    string emailSubject = "";
                    string emailBody = "";
                    string recipientEmail = notification.RecipientEmail;

                    if (notification.RecipientType == "APPLICANT")
                    {
                        emailSubject = "Pension Claim Status Update";
                        emailBody = $"Dear {claim.Name},\n\nYour pension claim has been rejected.\n\nClaim Details:\nName: {claim.Name}\nCompany: {claim.CompanyName}";
                        recipientEmail = claim.Email; // Send to applicant's email
                    }
                    else // For other recipients
                    {
                        emailSubject = "Pension Claim Rejected";
                        emailBody = $"A pension claim for {claim.Name} has been rejected.\n\nClaim Details:\nName: {claim.Name}\nEmail: {claim.Email}\nCompany: {claim.CompanyName}";
                    }

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await _emailService.SendEmailAsync(
                            recipientEmail,
                            emailSubject,
                            emailBody
                        );
                    }
                }
            }

            return claim;
        }

        public async Task<Claim> FinalApproveClaimAsync(Guid id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
                throw new KeyNotFoundException($"Claim with ID {id} not found");

            claim.Status = "FINAL_APPROVED";
            claim.FinalApprovalDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Send emails for Final Approval workflow step
            var finalApprovalNotifications = await _emailPreferenceService.GetWorkflowNotificationsByStepAsync("FINANCE_APPROVAL");

            foreach (var notification in finalApprovalNotifications)
            {
                if (notification.SendNotification)
                {
                    string emailSubject = "";
                    string emailBody = "";
                    string recipientEmail = notification.RecipientEmail;

                    if (notification.RecipientType == "APPLICANT")
                    {
                        emailSubject = "Pension Claim Approved!";
                        emailBody = $"Dear {claim.Name},\n\nYour pension claim has been approved!\n\nClaim Details:\nName: {claim.Name}\nCompany: {claim.CompanyName}\nStatus: Final Approval Granted";
                        recipientEmail = claim.Email; // Send to applicant's email
                    }
                    else // For other recipients (finance, etc.)
                    {
                        emailSubject = "Pension Claim Final Approval";
                        emailBody = $"A pension claim for {claim.Name} has received final approval.\n\nClaim Details:\nName: {claim.Name}\nEmail: {claim.Email}\nCompany: {claim.CompanyName}\nStatus: Final Approval Granted";
                    }

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await _emailService.SendEmailAsync(
                            recipientEmail,
                            emailSubject,
                            emailBody
                        );
                    }
                }
            }

            return claim;
        }

        public async Task<Claim> FinalRejectClaimAsync(Guid id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
                throw new KeyNotFoundException($"Claim with ID {id} not found");

            claim.Status = "FINAL_REJECTED";
            await _context.SaveChangesAsync();

            // Send emails for Final Rejection workflow step
            var finalRejectedNotifications = await _emailPreferenceService.GetWorkflowNotificationsByStepAsync("FINAL_REJECTED");

            foreach (var notification in finalRejectedNotifications)
            {
                if (notification.SendNotification)
                {
                    string emailSubject = "";
                    string emailBody = "";
                    string recipientEmail = notification.RecipientEmail;

                    if (notification.RecipientType == "APPLICANT")
                    {
                        emailSubject = "Pension Claim Status Update";
                        emailBody = $"Dear {claim.Name},\n\nYour pension claim has been rejected in the final review.\n\nClaim Details:\nName: {claim.Name}\nCompany: {claim.CompanyName}";
                        recipientEmail = claim.Email; // Send to applicant's email
                    }
                    else // For other recipients
                    {
                        emailSubject = "Pension Claim Final Rejection";
                        emailBody = $"A pension claim for {claim.Name} has been rejected in the final review.\n\nClaim Details:\nName: {claim.Name}\nEmail: {claim.Email}\nCompany: {claim.CompanyName}";
                    }

                    if (!string.IsNullOrEmpty(recipientEmail))
                    {
                        await _emailService.SendEmailAsync(
                            recipientEmail,
                            emailSubject,
                            emailBody
                        );
                    }
                }
            }

            return claim;
        }
    }
} 