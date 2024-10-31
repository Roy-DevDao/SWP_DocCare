using System;
using Microsoft.AspNetCore.Http;

namespace test2.Models.AdminModel
{
    public class EditBlogViewModel
    {
        public string BlogId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Content { get; set; }

        public string Image { get; set; } = null!;

        public IFormFile? ImageUpload { get; set; } // File for image upload

        public DateTime? CreateDate { get; set; }

        public string? CreateBy { get; set; }
    }
}
