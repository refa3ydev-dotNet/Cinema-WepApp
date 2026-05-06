using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DTOs.Admin
{
    public class RecentActivityResult
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = "info";
        public DateTime CreatedAt { get; set; }
    }
}
