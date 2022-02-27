using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DijitalCard.WebUI.Site.Authorize
{
    public class ClaimsHelpers
    {
        public IHttpContextAccessor httpContextAccessor;

        public ClaimsHelpers(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var claims = this.httpContextAccessor.HttpContext.User.Claims;
            if (claims == null)
                return -1;

            var userId = claims.FirstOrDefault(x => x.Type.Equals("UserId", System.StringComparison.OrdinalIgnoreCase))?.Value;
            if (userId == null)
                return -1;

            return int.Parse(userId);
        }
    }
}
