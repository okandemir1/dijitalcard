namespace DijitalCard.WebUI.Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RegisterModel
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RPassword { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
