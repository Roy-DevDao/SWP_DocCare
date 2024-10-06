namespace test2.Models
{
    public class Statistic
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public Statistic() { }

        public Statistic(DateTime date, int count)
        {
            this.Date = date;
            this.Count = count;
        }
    }

}
