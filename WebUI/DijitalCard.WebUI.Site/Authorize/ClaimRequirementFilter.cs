using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DijitalCard.Data;
using DijitalCard.WebUI.Infrastructure.Cache;

namespace DijitalCard.WebUI.Site.Authorize
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {   
        CacheHelper cacheHelper;
        
        public ClaimRequirementFilter(CacheHelper cacheHelper)
        {
            this.cacheHelper = cacheHelper;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.Claims.FirstOrDefault(x=>x.Type == System.Security.Claims.ClaimTypes.Role);
            if(role == null ||role.Value == "")
            {
                context.Result = new RedirectResult("/Home/Login");
                return;
            }

            //Bu role idli kullanıcı adminse devam et sorgulama
            if(role.Value == "1")
                return;
        }
    }
}
