namespace test2.Models.AdminModel
{
    public class EditSpecialtyViewModel
    {
        public string SpecialtyId { get; set; } = null!;
        public string SpecialtyName { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string SpecialtyImg { get; set; } = null!;
        public IFormFile? SpecialtyImgUpload { get; set; } // Để tải file ảnh lên
    }

}
