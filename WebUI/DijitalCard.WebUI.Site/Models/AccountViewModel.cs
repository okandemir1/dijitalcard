namespace DijitalCard.WebUI.Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountViewModel
    {
        public AccountViewModel()
        {
            Account = new Model.Account();
            AccountPlatforms = new List<Model.AccountPlatform>();
        }

        public Model.Account Account { get; set; }
        public List<Model.AccountPlatform> AccountPlatforms { get; set; }
        public bool HasLogged { get; set; }
    }
}
