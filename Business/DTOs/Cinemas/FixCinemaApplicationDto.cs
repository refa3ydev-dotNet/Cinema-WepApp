using Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.DTOs.Cinemas
{
    public class FixCinemaApplicationDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
        public string? RejectionReason { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public IFormFile? NewLogo { get; set; }
        public IFormFile? NewBackground { get; set; }
        //public IFormFile? CommercialRegisterDocument { get; set; }
        public string? LogoPath { get; set; }
        public string? BackgroundPicturePath { get; set; }
        //public string? CommercialRegisterDocumentPath { get; set; }
    }
}
