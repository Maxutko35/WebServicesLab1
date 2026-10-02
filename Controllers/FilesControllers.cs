using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace WebServicesLab1.Controllers
{
    public class FilesController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        
        public FilesController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        
        public IActionResult Index()
        {
            string path = Path.Combine(_appEnvironment.WebRootPath, "Files");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var files = Directory.GetFiles(path).Select(Path.GetFileName).ToList();
            
            return View(files);
        }
        
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = Path.Combine(_appEnvironment.WebRootPath, "Files");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                string filePath = Path.Combine(path, uploadedFile.FileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            
            return RedirectToAction("Index");
        }
        
        public IActionResult ViewFile(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return NotFound();

            string filePath = Path.Combine(_appEnvironment.WebRootPath, "Files", filename);
            if (!System.IO.File.Exists(filePath)) return NotFound("Файл не знайдено");
            
            string contentType = "application/octet-stream";
            string ext = Path.GetExtension(filename).ToLower();
            if (ext == ".pdf") contentType = "application/pdf";
            else if (ext == ".jpg" || ext == ".jpeg") contentType = "image/jpeg";
            else if (ext == ".png") contentType = "image/png";
            else if (ext == ".txt") contentType = "text/plain";
            
            return PhysicalFile(filePath, contentType);
        }
    }
}