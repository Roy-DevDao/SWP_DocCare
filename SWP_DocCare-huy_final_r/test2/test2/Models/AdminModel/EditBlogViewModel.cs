using System;
using Microsoft.AspNetCore.Http;

namespace test2.Models.AdminModel
{
    public class EditBlogViewModel
    {
        public string BlogId { get; set; } = null!;

        public string Title { get; set; } = null!; // Title of the blog post

        public string ShortDescription { get; set; } = null!; // Short description of the blog post

        public string Content { get; set; } = null!; // Main content of the blog post

        public string Image { get; set; } = null!; // Current image URL
        public DateTime? CreateDate { get; set; } = null!;

        public IFormFile? ImageUpload { get; set; } // For uploading a new image
    }
}
