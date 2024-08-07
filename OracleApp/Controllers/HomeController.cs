using Microsoft.AspNetCore.Mvc;
using OracleApp.Models;
using OracleApp.Service.Interface;
using System.Diagnostics;

namespace OracleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileInfoService _fileInfoService;

        public HomeController(ILogger<HomeController> logger, IFileInfoService fileInfoService)
        {
            _logger = logger;
            _fileInfoService = fileInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ShowFileInformation()
        {
            var fileInfo = await _fileInfoService.GetAllFileInformation();
            return View(fileInfo);
        }
    }
}
