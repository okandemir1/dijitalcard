namespace DijitalCard.WebUI.Site.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using DijitalCard.WebUI.Site.Models;
    using DijitalCard.Data;

    public class HomeController : Controller
    {
        PlatformData _platformData;

        public HomeController(PlatformData _platformData)
        {
            this._platformData = _platformData;
        }

        public IActionResult Index()
        {
            var platforms = _platformData.GetBy(x => x.IsActive && !x.IsDeleted);
            return View(platforms);
        }
    }
}
