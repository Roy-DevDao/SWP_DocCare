using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Needed for IFormFile

namespace test2.Models.AdminModel
{
    public class AddBlogViewModel
    {
        [Required]
        public string BlogId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public string? Image { get; set; }

        [Display(Name = "Short Description")]
        public string? ShortDescription { get; set; }

        [Display(Name = "Content")]
        public string? Content { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Created By")]
        public string? CreateBy { get; set; }

        [Display(Name = "Blog Image")]
        public IFormFile? ImageUpload { get; set; } // File for image upload
    }
}
