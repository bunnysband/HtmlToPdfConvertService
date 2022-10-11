using HtmlToPdfConvertService.Models;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToPdfConvertService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IItemManager itemManager;
        private readonly string uploadFolderPath;

        public HomeController(ILogger<HomeController> logger, IItemManager itemManager, IConfiguration appConfiguration)
        {
            this.logger = logger;
            this.itemManager = itemManager;
            uploadFolderPath = Path.Combine(appConfiguration["FileStorageFolderPath"], "uploads");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }
        }

        public IActionResult Index()
        {
            ViewBag.Guid = Guid.Empty;
            return View();
        }

        public IActionResult Convert()
        {
            var request = ControllerContext.HttpContext.Request;
            IFormFile fileToUpload = request.Form.Files.SingleOrDefault();
            if (fileToUpload == null)
            {
                return BadRequest("No files to upload");
            }
            var identity = Guid.NewGuid();
            var filePath = Path.Combine(uploadFolderPath, $"{identity}_{fileToUpload.FileName}");

            var convertedPath = UploadFile(fileToUpload, filePath)
                .ContinueWith(task => itemManager.CreateNewItem(identity, fileToUpload.FileName))
                .ContinueWith(task => itemManager.ConvertFile(identity, filePath)).Result;
            ViewBag.Guid = identity;
            return View("Index");
        }

        public IActionResult Download(Guid id)
        {
            var filePath = itemManager.GetConvertedFile(id);
            return PhysicalFile(filePath, "application/pdf");
        }

        private async Task UploadFile(IFormFile file, string localFilePath)
        {
            using (var fileStream = new FileStream(localFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}