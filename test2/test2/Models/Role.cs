namespace test2.Models
{
    public class Role
    {
        private int roleId { get; set; }
        private string name { get; set; }

        private string note { get; set; }

        public Role() { }

        public Role(int roleId, string name)
        {
            this.roleId = roleId;
            this.name = name;
        }

        public Role(int roleId, string name, string note)
        {
            this.roleId = roleId;
            this.name = name;
            this.note = note;
        }


        public Role(int roleId)
        {
            this.roleId = roleId;
        }

        public Role(string name)
        {
            this.name = name;
        }
    }
}
