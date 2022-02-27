namespace DijitalCard.WebUI.Management.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DijitalCard.Data;
    using DijitalCard.WebUI.Management.Authorize;
    using DijitalCard.WebUI.Management.Models;

    [Authorize]
    public class PlatformController : Controller
    {
        PlatformData _platformData;

        public PlatformController(PlatformData _platformData)
        {
            this._platformData = _platformData;
        }

        public IActionResult Index()
        {
            var platforms = _platformData.GetBy(x => !x.IsDeleted);
            return View(platforms);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var platform = new Model.Platform()
            {
                IsActive = false,
                IsDeleted = false,
                Name = "",
                ImagePath = "",
            };

            return View(platform);
        }

        [HttpPost]
        public IActionResult Add(Model.Platform platform, IFormFile file)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(platform.Name)) errors.Add("Platform Adı Boş Bırakılamaz");
            if(file == null || file.Length == 0) errors.Add("Resim Boş Bırakılamaz");
            if (errors.Count() > 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", errors);
                return View(platform);
            }

            var extension = Path.GetExtension(file.FileName).Trim('.').ToLower();
            if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", "Resim sadece jpg, png yada jpeg olabilir");
                return View(platform);
            }

            var local_image_dir = $"wwwroot/_uploads/platforms";
            var local_image_path = $"{local_image_dir}/{file.FileName}";

            if (!Directory.Exists(Path.Combine(local_image_dir)))
                Directory.CreateDirectory(Path.Combine(local_image_dir));

            using (Stream fileStream = new FileStream(local_image_path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            platform.ImagePath = local_image_path;

            var operationResult = _platformData.Insert(platform);
            if (operationResult.IsSucceed)
            {
                ViewBag.Result = new ViewModelResult(true, "Yeni platform eklendi");
                return View(new Model.Platform());
            }

            ViewBag.Result = new ViewModelResult(false, operationResult.Message);
            return View(platform);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var platform = _platformData.GetByKey(id);
            if (platform == null)
                return RedirectToAction("Index", "Home", new { q = "platform-bulunamadi" });

            return View(platform);
        }

        [HttpPost]
        public IActionResult Edit(Model.Platform platform, IFormFile file)
        {
            var errors = new List<string>();
            var modelInDb = _platformData.GetByKey(platform.Id);

            if (string.IsNullOrEmpty(platform.Name)) errors.Add("Platform Adı Boş Bırakılamaz");
            if (errors.Count() > 0)
            {
                ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", errors);
                return View(platform);
            }

            var exist_platform = _platformData.GetBy(x => x.Name == platform.Name && !x.IsDeleted && x.Id != platform.Id).FirstOrDefault();
            if (exist_platform != null)
            {
                ViewBag.Result = new ViewModelResult(false, "Bu zaten kayıtlı");
                return View(platform);
            }

            if(file != null && file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).Trim('.').ToLower();
                if (!(new[] { "jpg", "png", "jpeg" }.Contains(extension)))
                {
                    ViewBag.Result = new ViewModelResult(false, "Hata Oluştu", "Resim sadece jpg, png yada jpeg olabilir");
                    return View(platform);
                }

                var local_image_dir = $"wwwroot/_uploads/platforms";
                var local_image_path = $"{local_image_dir}/{file.FileName}";

                if (!Directory.Exists(Path.Combine(local_image_dir)))
                    Directory.CreateDirectory(Path.Combine(local_image_dir));

                using (Stream fileStream = new FileStream(local_image_path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                modelInDb.ImagePath = local_image_path;
            }

            modelInDb.Name = platform.Name;
            modelInDb.IsActive = platform.IsActive;

            var operationResult = _platformData.Update(modelInDb);
            if (operationResult.IsSucceed)
            {
                ViewBag.Result = new ViewModelResult(true, "Platform Güncellendi.");
                return View(platform);
            }

            ViewBag.Result = new ViewModelResult(false, operationResult.Message);
            return View(platform);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var platform = _platformData.GetByKey(id);
            if (platform == null)
                return RedirectToAction("Index", "Platform", new { q = "platform-bulunamadi" });

            platform.IsDeleted = true;
            var operationResult = _platformData.Update(platform);
            if (operationResult.IsSucceed)
                return RedirectToAction("Index", "Platform", new { q = "platform-silindi" });
            else
                return RedirectToAction("Index", "Platform", new { q = "platform-silemedim" });
        }

        [HttpGet]
        public IActionResult IsActive(int id)
        {
            var platform = _platformData.GetByKey(id);
            if (platform == null)
                return RedirectToAction("Index", "Platform", new { q = "platform-bulunamadi" });

            platform.IsActive = true;
            var operationResult = _platformData.Update(platform);
            if (operationResult.IsSucceed)
                return RedirectToAction("Index", "Platform", new { q = "platform-aktif" });
            else
                return RedirectToAction("Index", "Platform", new { q = "platform-aktif-edilmedi" });
        }

        [HttpGet]
        public IActionResult IsPassive(int id)
        {
            var platform = _platformData.GetByKey(id);
            if (platform == null)
                return RedirectToAction("Index", "Platform", new { q = "platform-bulunamadi" });

            platform.IsActive = false;
            var operationResult = _platformData.Update(platform);
            if (operationResult.IsSucceed)
                return RedirectToAction("Index", "Platform", new { q = "platform-pasif" });
            else
                return RedirectToAction("Index", "Platform", new { q = "platform-pasif-edilmedi" });
        }
    }
}
