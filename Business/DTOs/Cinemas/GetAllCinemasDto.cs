using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Cinemas
{
    public class GetAllCinemasDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public IFormFile? BackgroundPicture { get; set; }
        public string? BackgroundPath { get; set; }
        public string? LogoPath { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public IFormFile? Logo { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? RejectionReason { get; set; }

    }
}
