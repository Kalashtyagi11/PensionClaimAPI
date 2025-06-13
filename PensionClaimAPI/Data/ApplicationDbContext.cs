using Microsoft.EntityFrameworkCore;
using PensionClaimAPI.Models;

namespace PensionClaimAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<WorkflowNotification> WorkflowNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkflowNotification>().HasData(
                new WorkflowNotification { Id = 1, WorkflowStep = "CLAIM_SUBMISSION", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "CLAIM_MANAGER_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 2, WorkflowStep = "CLAIM_SUBMISSION", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "CLAIM_MANAGER_PRIYA", SendNotification = true },
                new WorkflowNotification { Id = 3, WorkflowStep = "CLAIM_SUBMISSION", RecipientEmail = "harshi2293@gmail.com", RecipientType = "CLAIM_MANAGER_HARSHITA", SendNotification = false },
                new WorkflowNotification { Id = 4, WorkflowStep = "CLAIM_SUBMISSION", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = false },

                new WorkflowNotification { Id = 5, WorkflowStep = "FIELD_OFFICER_CONFIRMATION", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "FIELD_OFFICER_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 6, WorkflowStep = "FIELD_OFFICER_CONFIRMATION", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "FIELD_OFFICER_PRIYA", SendNotification = true },
                new WorkflowNotification { Id = 7, WorkflowStep = "FIELD_OFFICER_CONFIRMATION", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = true },

                new WorkflowNotification { Id = 8, WorkflowStep = "FINANCE_APPROVAL", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "FINANCE_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 9, WorkflowStep = "FINANCE_APPROVAL", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "FINANCE_PRIYA", SendNotification = true },
                new WorkflowNotification { Id = 10, WorkflowStep = "FINANCE_APPROVAL", RecipientEmail = "harshi2293@gmail.com", RecipientType = "FINANCE_HARSHITA", SendNotification = true },
                new WorkflowNotification { Id = 11, WorkflowStep = "FINANCE_APPROVAL", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = true },

                new WorkflowNotification { Id = 12, WorkflowStep = "DISBURSEMENT", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "DISBURSEMENT_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 13, WorkflowStep = "DISBURSEMENT", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "DISBURSEMENT_PRIYA", SendNotification = false },
                new WorkflowNotification { Id = 14, WorkflowStep = "DISBURSEMENT", RecipientEmail = "harshi2293@gmail.com", RecipientType = "DISBURSEMENT_HARSHITA", SendNotification = true },
                new WorkflowNotification { Id = 15, WorkflowStep = "DISBURSEMENT", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = true }
            );

            modelBuilder.Entity<WorkflowNotification>().HasData(
                new WorkflowNotification { Id = 16, WorkflowStep = "EI_CLAIM_SUBMISSION", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "EI_CLAIM_MANAGER_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 17, WorkflowStep = "EI_CLAIM_SUBMISSION", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "EI_CLAIM_MANAGER_PRIYA", SendNotification = true },
                new WorkflowNotification { Id = 18, WorkflowStep = "EI_CLAIM_SUBMISSION", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = true },

                new WorkflowNotification { Id = 19, WorkflowStep = "EI_FIELD_OFFICER_CONFIRMATION", RecipientEmail = "anish.s@mishainfotech.com", RecipientType = "EI_FIELD_OFFICER_ANISH", SendNotification = true },
                new WorkflowNotification { Id = 20, WorkflowStep = "EI_FIELD_OFFICER_CONFIRMATION", RecipientEmail = "priya.a@mishainfotech.com", RecipientType = "EI_FIELD_OFFICER_PRIYA", SendNotification = true },
                new WorkflowNotification { Id = 21, WorkflowStep = "EI_FIELD_OFFICER_CONFIRMATION", RecipientEmail = "", RecipientType = "APPLICANT", SendNotification = true }
            );
        }
    }
} 