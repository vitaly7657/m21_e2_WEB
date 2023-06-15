using m21_e2_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;


namespace m21_e2_WEB.Controllers
{
    public class HomeController : Controller //контроллер управления главной страницей приложения
    {
        public HttpClient httpClient { get; set; }
        public HomeController()
        {
            httpClient = new HttpClient();
        }

        //главная страница с выводом всех абонентов
        public IActionResult Index()
        {
            string url = @"https://localhost:44380/api/application/subscribers/";
            string json = httpClient.GetStringAsync(url).Result;
            return View(JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(json));
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
    }
}