using Microsoft.AspNetCore.Mvc;
using WebServicesLab1.Models;
using WebServicesLab1.Services;
using System.Threading.Tasks;

namespace WebServicesLab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        
        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
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
            }
            
            return RedirectToAction("Index"); 
        }
    }
}