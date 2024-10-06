using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OracleApp.Models;
using OracleApp.Service.Interface;
using System.Diagnostics;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace OracleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileInfoService _fileInfoService;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger, IFileInfoService fileInfoService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _fileInfoService = fileInfoService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<JsonResult> GetToken()
        {
            if (!_memoryCache.TryGetValue("Token", out TokenResult tokenResult))
            {
                var result = await GetTokenFromProvider();
                if (result?.StatusCode == 200)
                    _memoryCache.Set<string>("Token", result.AccessToken, TimeSpan.FromMinutes(10));
            }
            return Json(tokenResult);
        }


        private async Task<TokenResult> GetTokenFromProvider()
        {
            HttpClient httpClient = new HttpClient();

            // Prepare the form data as key-value pairs
            var formData = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };

            // Encode the data as application/x-www-form-urlencoded
            var content = new FormUrlEncodedContent(formData);
            HttpResponseMessage response = await httpClient.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                var tokenResult = JsonConvert.DeserializeObject<TokenResult>(await response.Content.ReadAsStringAsync());
                return new TokenResult { StatusCode = (int)response.StatusCode, AccessToken = tokenResult!.AccessToken };

            }
            else
                return new TokenResult { StatusCode = (int)response.StatusCode, AccessToken = null };
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
            var filteredResult = result.Where(x => x.CaseNumber.Contains(searchFilter.GlobalSearchValue)).ToList();

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

    public class TokenResult
    {
        public int StatusCode { get; set; }

        public string AccessToken { get; set; }
    }
}
