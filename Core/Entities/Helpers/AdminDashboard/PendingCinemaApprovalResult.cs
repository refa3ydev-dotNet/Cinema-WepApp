using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class PendingCinemaApprovalResult
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ImageUrl { get; set; }
    }
}
