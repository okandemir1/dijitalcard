using Microsoft.AspNetCore.Mvc;

namespace DijitalCard.WebUI.Site.Authorize
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { };
        }
    }
}
