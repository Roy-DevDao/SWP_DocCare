namespace test2.Models
{
    public class Staff
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Note { get; set; }
        public int Order { get; set; }

        public Staff() { }

        public Staff(int settingId, string settingName)
        {
            this.SettingId = settingId;
            this.SettingName = settingName;
        }

        public Staff(int id, string name, int settingId, bool status)
        {
            this.Id = id;
            this.Name = name;
            this.SettingId = settingId;
            this.Status = status;
        }

        public Staff(int id, string name, int settingId, bool status, string note, int order)
        {
            this.Id = id;
            this.Name = name;
            this.SettingId = settingId;
            this.Status = status;
            this.Note = note;
            this.Order = order;
        }

        public Staff(string name)
        {
            this.Name = name;
        }
    }
}
