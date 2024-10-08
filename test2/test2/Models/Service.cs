namespace test2.Models
{
    public class Service
    {
        public Staff staff { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public double Fee { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public bool Status { get; set; }
        public RateStar RateStar { get; set; }

        public Service() { }

        public Service(string img, string description)
        {
            this.Img = img;
            this.Description = description;
        }

        public Service(string title, Staff setting, RateStar rateStar, double fee, string description, int serviceId, string img)
        {
            this.Title = title;
            this.staff = setting;
            this.RateStar = rateStar;
            this.Fee = fee;
            this.Description = description;
            this.ServiceId = serviceId;
            this.Img = img;
        }

        public Service(Staff setting, bool status, int serviceId, string title, double fee, string description, string img)
        {
            this.staff = setting;
            this.Status = status;
            this.ServiceId = serviceId;
            this.Title = title;
            this.Fee = fee;
            this.Description = description;
            this.Img = img;
        }

        public Service(Staff setting, string title, double fee, string img)
        {
            this.staff = setting;
            this.Title = title;
            this.Fee = fee;
            this.Img = img;
        }

        public Service(int serviceId, string title, double fee)
        {
            this.ServiceId = serviceId;
            this.Title = title;
            this.Fee = fee;
        }

        public Service(int serviceId, string title)
        {
            if (serviceId != 0)
            {
                this.ServiceId = serviceId;
            }
            if (!string.IsNullOrEmpty(title))
            {
                this.Title = title;
            }
        }
    }

}
