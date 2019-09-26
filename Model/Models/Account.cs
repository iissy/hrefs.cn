using System;

namespace ASY.Hrefs.Model.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int Status { get; set; }
    }
}