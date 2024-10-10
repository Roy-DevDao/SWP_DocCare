using test2.Data;

namespace test2.Models
{
    public class DoctorViewModel
    {
        public string DoctorId { get; set; }
        public string Name { get; set; }
        public string DoctorImg { get; set; }
        public string Specialty { get; set; }
        public double Price { get; set; }
        public string Position { get; set; }
        public int NumberOfFeedbacks { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }

}
