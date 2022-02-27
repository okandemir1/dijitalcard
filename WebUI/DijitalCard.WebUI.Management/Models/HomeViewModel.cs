namespace DijitalCard.WebUI.Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Platforms = new List<Model.Platform>();
            Accounts = new List<Model.Account>();
        }
                
        public List<Model.Platform> Platforms { get; set; }
        public List<Model.Account> Accounts { get; set; }
    }
}
