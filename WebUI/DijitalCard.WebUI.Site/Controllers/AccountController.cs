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
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using DijitalCard.WebUI.Infrastructure.Encryption;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using DijitalCard.WebUI.Site.Authorize;

    public class AccountController : Controller
    {
        AccountData _accountData;
        AccountPlatformData _accountPlatformData;
        Encryption encryption;
        public string key = "A2jqix";
        int userId;

        public AccountController(AccountData _accountData, AccountPlatformData _accountPlatformData, ClaimsHelpers claimsHelpers)
        {
            this._accountData = _accountData;
            this._accountPlatformData = _accountPlatformData;
            encryption = new Encryption(key);
            userId = claimsHelpers.GetUserId();
        }

        [Authorize]
        public IActionResult Index(string username)
        {
            if(string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "Home", new { q = "username_bos" });

            var account = _accountData.GetBy(x => x.IsActive && !x.IsDeleted && x.Username == username).FirstOrDefault();
            if (account == null)
                return RedirectToAction("Index", "Home", new { q = "kullanici_bulunamadi" });

            var account_platforms = _accountPlatformData.GetBy(x => x.AccountId == account.Id);

            account.Fullname = encryption.Decrypt(account.Fullname);

            var model = new AccountViewModel()
            {
                Account = account,
                AccountPlatforms = account_platforms,
                HasLogged = userId <= 0 ? false : true,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel()
            {
                Email = "",
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model, IFormFile imagePath)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Username)) errors.Add("Kullanıcı adı boş bırakılamaz");
            if (string.IsNullOrEmpty(model.Fullname)) errors.Add("Ad Soyad boş bırakılamaz");
            if (string.IsNullOrEmpty(model.Password)) errors.Add("Şifre boş bırakılamaz");
            if (string.IsNullOrEmpty(model.RPassword)) errors.Add("Şifre tekrar boş bırakılamaz");
            if (model.Password != model.RPassword) errors.Add("Şifreler Uyuşmuyor");
            if (string.IsNullOrEmpty(model.Email)) errors.Add("Mail adresi boş bırakılamaz");

            if(errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View(model);
            }

            var mail_exist = _accountData.GetBy(x => x.Email == model.Email && x.IsActive && !x.IsDeleted).FirstOrDefault();
            if(mail_exist != null)
            {
                errors.Add("Bu mail adresi ile daha önce kayıt oluşturulmuş");
                ViewBag.Errors = errors;
                return View(model);
            }


            if (imagePath != null && imagePath.Length > 0)
            {
                var extension = Path.GetExtension(imagePath.FileName).Trim('.').ToLower();
                if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
                {
                    errors.Add("Resim sadece jpg, png yada jpeg olabilir");
                    ViewBag.Errors = errors;
                    return View(model);
                }

                var local_image_dir = $"wwwroot/_uploads/avatars";
                var local_image_path = $"{local_image_dir}/{imagePath.FileName}";

                if (!Directory.Exists(Path.Combine(local_image_dir)))
                    Directory.CreateDirectory(Path.Combine(local_image_dir));

                using (Stream fileStream = new FileStream(local_image_path, FileMode.Create))
                {
                    imagePath.CopyTo(fileStream);
                }

                model.ImagePath = local_image_path;
            }

            var account_model = new Model.Account()
            {
                CreateDate = DateTime.Now,
                Email = encryption.Encrypt(model.Email),
                Fullname = encryption.Encrypt(model.Fullname),
                ImagePath = model.ImagePath ?? "",
                IsActive = true,
                IsDeleted = false,
                Password = encryption.Encrypt(model.Password),
                Title = model.Title ?? "",
                Username = model.Username,
            };

            var insert_account = _accountData.Insert(account_model);
            if (insert_account.IsSucceed)
            {
                return RedirectToAction("Login", "Account", new { q = "success" });
            }

            errors.Add("Kayıt eklenemedi. Destek bölümünden iletişime geçebilirisniz.");
            ViewBag.Errors = errors;
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
        public IActionResult Login(LoginModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Username)) errors.Add("Kullanıcı adı boş bırakılamaz");
            if (string.IsNullOrEmpty(model.Password)) errors.Add("Şifre boş bırakılamaz");

            if (errors.Count > 0)
            {
                ViewBag.Errors = errors;
                return View(model);
            }

            var enc_pass = encryption.Encrypt(model.Password);

            var user_exist = _accountData.GetBy(x => x.Username == model.Username && x.Password == enc_pass && x.IsActive && !x.IsDeleted).FirstOrDefault();
            if (user_exist == null)
            {
                errors.Add("Kayıtlı böyle bir kullanıcı bulunamadı.");
                ViewBag.Errors = errors;
                return View(model);
            }

            user_exist.Username = encryption.Decrypt(user_exist.Fullname);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user_exist.Username),
                new Claim("UserId", user_exist.Id.ToString()),
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
    }
}
