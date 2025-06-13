using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PensionClaimAPI.Data;
using PensionClaimAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace PensionClaimAPI.Services
{
    public class EmailPreferenceService : IEmailPreferenceService
    {
        private readonly ApplicationDbContext _context;

        public EmailPreferenceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowNotification> GetWorkflowNotificationAsync(string workflowStep, string recipientType)
        {
            return await _context.WorkflowNotifications
                .FirstOrDefaultAsync(n => n.WorkflowStep == workflowStep && n.RecipientType == recipientType);
        }

        public async Task UpdateWorkflowNotificationAsync(WorkflowNotification notification)
        {
            _context.WorkflowNotifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkflowNotification>> GetAllWorkflowNotificationsAsync()
        {
            return await _context.WorkflowNotifications.ToListAsync();
        }

        public async Task<IEnumerable<WorkflowNotification>> GetWorkflowNotificationsByStepAsync(string workflowStep)
        {
            return await _context.WorkflowNotifications
                .Where(n => n.WorkflowStep == workflowStep).ToListAsync();
        }

        public async Task<WorkflowNotification> CreateWorkflowNotificationAsync(WorkflowNotification notification)
        {
            _context.WorkflowNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}