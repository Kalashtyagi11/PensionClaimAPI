using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PensionClaimAPI.Services;
using PensionClaimAPI.Models;
using System.Collections.Generic;

namespace PensionClaimAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailPreferenceController : ControllerBase
    {
        private readonly IEmailPreferenceService _emailPreferenceService;

        public EmailPreferenceController(IEmailPreferenceService emailPreferenceService)
        {
            _emailPreferenceService = emailPreferenceService;
        }

        [HttpGet("{workflowStep}/{recipientType}")]
        public async Task<ActionResult<WorkflowNotification>> GetWorkflowNotification(
            string workflowStep, string recipientType)
        {
            var notification = await _emailPreferenceService.GetWorkflowNotificationAsync(workflowStep, recipientType);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkflowNotification>>> GetAllWorkflowNotifications()
        {
            var notifications = await _emailPreferenceService.GetAllWorkflowNotificationsAsync();
            return Ok(notifications);
        }

        [HttpPost]
        public async Task<ActionResult<WorkflowNotification>> CreateWorkflowNotification([FromBody] WorkflowNotification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification data is null.");
            }

            var createdNotification = await _emailPreferenceService.CreateWorkflowNotificationAsync(notification);
            return CreatedAtAction(nameof(GetWorkflowNotification), 
                new { workflowStep = createdNotification.WorkflowStep, recipientType = createdNotification.RecipientType }, 
                createdNotification);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWorkflowNotification([FromBody] WorkflowNotification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification data is null.");
            }

            var existingNotification = await _emailPreferenceService.GetWorkflowNotificationAsync(
                notification.WorkflowStep, notification.RecipientType);

            if (existingNotification == null)
            {
                return NotFound("Workflow notification not found.");
            }

            existingNotification.SendNotification = notification.SendNotification;
            existingNotification.RecipientEmail = notification.RecipientEmail;
            existingNotification.RecipientType = notification.RecipientType;
            existingNotification.WorkflowStep = notification.WorkflowStep;

            await _emailPreferenceService.UpdateWorkflowNotificationAsync(existingNotification);
            return NoContent();
        }
    }
} 