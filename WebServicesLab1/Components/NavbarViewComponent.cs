using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebServicesLab1.Models;

namespace WebServicesLab1.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<MenuItem>
            {
                new MenuItem { LocalizationKey = "Home", Controller = "Home", Action = "Index" },
                new MenuItem { LocalizationKey = "About", Controller = "Home", Action = "About" },
                new MenuItem { LocalizationKey = "Typography", Controller = "Home", Action = "Typography" },
                new MenuItem { LocalizationKey = "Contacts", Controller = "Home", Action = "Contacts" }
            };

            return View(menuItems);
        }
    }
}