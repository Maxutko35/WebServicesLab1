using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebServicesLab1.Models;
using WebServicesLab1.Services;

namespace WebServicesLab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IEmailSender emailSender, ILogger<HomeController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Contacts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                string subject = $"Нове повідомлення з сайту від {model.Name}";
                await _emailSender.SendEmailAsync(model.Email, subject, model.Message);
                _logger.LogInformation("ПОДІЯ: Успішно відправлено лист на адресу {Email} від клієнта {Name}", model.Email, model.Name);
            }
            
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}