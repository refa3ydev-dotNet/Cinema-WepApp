using Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Business.DTOs.Cinemas
{
    public class UpdateCinemaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ApprovalStatus ApprovalStatus { get; set; }
        public IFormFile? BackgroundPicture { get; set; }
        public string? BackgroundPath { get; set; }
        public string? LogoPath { get; set; }
        public IFormFile? Logo { get; set; }

    }
}
