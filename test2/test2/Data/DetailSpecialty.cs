namespace test2.Data
{
    public class DetailSpecialty
    {
        public string DetailId { get; set; }
        public string SpecialtyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Navigation property to Specialty
        public virtual Specialty Specialty { get; set; }
    }
}
