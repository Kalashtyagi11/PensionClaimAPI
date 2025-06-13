using System;
using System.ComponentModel.DataAnnotations;

namespace PensionClaimAPI.Models
{
    public class WorkflowNotification
    {
        [Key]
        public int Id { get; set; }
        public string WorkflowStep { get; set; }
        public string RecipientType { get; set; }
        public string RecipientEmail { get; set; }
        public bool SendNotification { get; set; }
    }
} 