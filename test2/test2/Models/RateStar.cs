namespace test2.Models
{
    public class RateStar
    {
        public int Id { get; set; }
        public string Feedback { get; set; }
        public int Star { get; set; }
        public DateTime Date { get; set; }
        public int CountFeedback { get; set; }
        public Account User { get; set; }

        public RateStar()
        {
        }

        public RateStar(Account user, int star, string feedback, DateTime date)
        {
            this.User = user;
            this.Star = star;
            this.Feedback = feedback;
            this.Date = date;
        }

        public RateStar(int star, int countFeedback)
        {
            this.Star = star;
            this.CountFeedback = countFeedback;
        }

        public RateStar(Account user, int star, string feedback)
        {
            this.User = user;
            this.Star = star;
            this.Feedback = feedback;
        }
    }
}
