using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetFilesInfo([FromBody] SearchFilter searchFilter)
        {
            var result = await _fileInfoService.GetAllFileInformation();
            var filteredResult =  result.Where(x => x.CaseNumber.Contains(searchFilter.GlobalSearchValue)).ToList();

            var paginationResult = filteredResult.Skip(searchFilter.Start).Take(searchFilter.Length).ToList();
            return Json(new { draw = searchFilter.Draw, recordsFiltered = filteredResult.Count, recordsTotal = filteredResult.Count, data = paginationResult });
        }
    }

    public class SearchFilter
    {
        public string GlobalSearchValue { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }

        public int Length { get; set; }
    }
}
