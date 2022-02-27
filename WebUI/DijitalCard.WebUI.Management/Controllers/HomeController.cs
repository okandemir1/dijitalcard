namespace DijitalCard.WebUI.Management.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using DijitalCard.Data;
    using DijitalCard.WebUI.Management.Authorize;
    using DijitalCard.WebUI.Management.Models;

    public class HomeController : Controller
    {
        PlatformData _platformData;
        AccountData _accountData;

        public HomeController(PlatformData _platformData, AccountData _accountData)
        {
            this._platformData = _platformData;
            this._accountData = _accountData;
        }

        [Authorize]
        public IActionResult Index()
        {
            var platforms = _platformData.GetByPage(x => !x.IsDeleted,1,10);
            var accounts = _accountData.GetByPage(x => !x.IsDeleted,1,10);

            var model = new HomeViewModel()
            {
                Accounts = accounts,
                Platforms = platforms,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel()
            {
                Password = "",
                Username = "",
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var errors = new List<string>();
            var return_model = new LoginModel();

            if (string.IsNullOrEmpty(username)) errors.Add("Kullanıcı Boş Bırakılamaz");
            if (string.IsNullOrEmpty(password)) errors.Add("Şifre Boş Bırakılamaz");
            if (errors.Count() > 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", errors);
                return View(return_model);
            }

            if(username != "manager" && password != "123456")
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", "Kullanıcı Bulunamadı");
                return View(return_model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Okan Demir"),
                new Claim("AuthorId", "1"),
                new Claim(ClaimTypes.Role, "1")
            };

            var clasim_identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var auth_properties = new AuthenticationProperties
            {
                ExpiresUtc = System.DateTimeOffset.UtcNow.AddMinutes(60),
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(clasim_identity),
                auth_properties
            );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login","Home");
        }

        [HttpGet]
        public IActionResult _403()
        {
            return View();
        }
    }
}
