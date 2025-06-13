namespace PensionClaimAPI.Models
{
    public class EmailPreference
    {
        public string Role { get; set; } // "MANAGER" or "APPROVER"
        public bool ReceiveEmails { get; set; }
    }
} 