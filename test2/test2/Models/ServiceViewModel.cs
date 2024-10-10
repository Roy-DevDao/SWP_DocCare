using test2.Data;

namespace test2.Models
{
    public class ServiceViewModel
    {
        public string SpecialtyId { get; set; }
        public string SpecialtyName { get; set; }
        public string SpecialtyImg { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }

}
