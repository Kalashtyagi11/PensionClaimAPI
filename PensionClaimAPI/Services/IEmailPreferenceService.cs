using System.Threading.Tasks;
using System.Collections.Generic;
using PensionClaimAPI.Models;

namespace PensionClaimAPI.Services
{
    public interface IEmailPreferenceService
    {
        Task<WorkflowNotification> GetWorkflowNotificationAsync(string workflowStep, string recipientType);
        Task UpdateWorkflowNotificationAsync(WorkflowNotification notification);
        Task<IEnumerable<WorkflowNotification>> GetAllWorkflowNotificationsAsync();
        Task<IEnumerable<WorkflowNotification>> GetWorkflowNotificationsByStepAsync(string workflowStep);
        Task<WorkflowNotification> CreateWorkflowNotificationAsync(WorkflowNotification notification);
    }
} 