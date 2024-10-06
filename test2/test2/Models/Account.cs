namespace test2.Models
{
    public class Account
    {
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public bool Status { get; set; }
        public string Captcha { get; set; }

        public Account()
        {
        }

        public Account(string username, Role role, string password, string name, bool gender, int phone, string email, string img, bool status)
        {
            this.Username = username;
            this.Role = role;
            this.Password = password;
            this.Name = name;
            this.Gender = gender;
            this.Phone = phone;
            this.Email = email;
            this.Img = img;
            this.Status = status;
        }

        public Account(string username, Role role, string name, bool gender, int phone, string email, string img, bool status)
        {
            this.Username = username;
            this.Role = role;
            this.Name = name;
            this.Gender = gender;
            this.Phone = phone;
            this.Email = email;
            this.Img = img;
            this.Status = status;
        }

        public Account(string username, Role role, string name, bool gender, int phone, string email, bool status)
        {
            this.Username = username;
            this.Role = role;
            this.Name = name;
            this.Gender = gender;
            this.Phone = phone;
            this.Email = email;
            this.Status = status;
        }

        public Account(string img, string username, string name, string email, bool gender, int phone)
        {
            this.Img = img;
            this.Username = username;
            this.Name = name;
            this.Email = email;
            this.Gender = gender;
            this.Phone = phone;
        }

        public Account(string username, string name, bool gender)
        {
            this.Username = username;
            this.Name = name;
            this.Gender = gender;
        }

        public Account(string username)
        {
            this.Username = username;
        }

        public Account(string username, string email, string captcha, string img)
        {
            if (!string.IsNullOrEmpty(username))
            {
                this.Username = username;
            }
            if (!string.IsNullOrEmpty(email))
            {
                this.Email = email;
            }
            if (!string.IsNullOrEmpty(captcha))
            {
                this.Captcha = captcha;
            }
            if (!string.IsNullOrEmpty(img))
            {
                this.Img = img;
            }
        }

        public Account(string img, string name, int phone, bool gender, string email)
        {
            this.Name = name;
            this.Gender = gender;
            this.Phone = phone;
            if (!string.IsNullOrEmpty(img))
            {
                this.Img = img;
            }
            if (!string.IsNullOrEmpty(email))
            {
                this.Email = email;
            }
        }

        public Account(string username, string name)
        {
            this.Username = username;
            this.Name = name;
        }
    }
}
