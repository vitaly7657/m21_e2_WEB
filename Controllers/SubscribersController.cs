using m21_e2_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace m21_e2_WEB.Controllers
{
    public class SubscribersController : Controller //контроллер управления абонентами
    {
        public HttpClient httpClient { get; set; }
        public SubscribersController()
        {
            httpClient = new HttpClient();
        }

        //получение всех абонентов
        public IActionResult GetSubscribers()
        {
            string url = @"https://localhost:44380/api/application/subscribers/";
            string json = httpClient.GetStringAsync(url).Result;
            return View(JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(json));
        }

        //получение одного конкретного абонента
        public IActionResult GetSubscriber(int? id)
        {
            string url = @$"https://localhost:44380/api/application/subscribers/{id}";
            string json = httpClient.GetStringAsync(url).Result;
            return View(JsonConvert.DeserializeObject<Subscriber>(json));
        }

        //страница создания абонента
        public IActionResult Create()
        {
            return View();
        }

        //создание абонента
        [HttpPost]
        public IActionResult CreateSubscriber(Subscriber subscriber)
        {
            string url = @"https://localhost:44380/api/application/subscribers/";

            var result = httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(subscriber), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Error", "Subscribers");
            }
            return RedirectToAction("Index", "Home");
        }

        //удаление абонента
        [HttpPost]
        public async Task<ActionResult> DeleteSubscriber(string id)
        {
            string url = @$"https://localhost:44380/api/application/subscribers/{id}";
            var result = await httpClient.DeleteAsync(url);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Error", "Subscribers");
            }
            return RedirectToAction("Index", "Home");
        }

        //страница редактирования абонента
        public IActionResult Edit(int? id)
        {
            string url = @$"https://localhost:44380/api/application/subscribers/{id}";
            string json = httpClient.GetStringAsync(url).Result;
            return View(JsonConvert.DeserializeObject<Subscriber>(json));
        }

        //редактирование абонента
        [HttpPost]
        public IActionResult EditSubscriber(Subscriber subscriber)
        {
            string url = @"https://localhost:44380/api/application/subscribers/";
            var r = httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(subscriber), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
            return RedirectToAction("Index", "Home");
        }

        //страница ошибки
        public IActionResult Error()
        {
            return View();
        }
    }
}
