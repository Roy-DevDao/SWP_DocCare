using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace test2.Models.AdminModel
{
    public class AddBlogViewModel
    {
        [Required]
        public string BlogId { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        [Display(Name = "Ảnh Blog")]
        public IFormFile? ImageUpload { get; set; } // File for image upload

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
    