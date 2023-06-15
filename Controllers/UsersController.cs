using m21_e2_WEB.Models;
using m21_e2_WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace m21_e2_WEB.Controllers
{
    public class UsersController : Controller //контроллер управления пользователями
    {
        public HttpClient httpClient { get; set; }
        public UsersController()
        {
            httpClient = new HttpClient();
        }

        //главная страница с выводом всех пользователей
        public IActionResult Index()
        {
            string url = @"https://localhost:44380/api/application/users";
            var json = httpClient.GetStringAsync(url).Result;
            return View(JsonConvert.DeserializeObject<List<User>>(json));
        }

        //страница создания пользователя
        public IActionResult Create()
        {
            return View();
        }

        //создание пользователя
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        { 
            User newUser = new User
            {
                UserName = model.UserName
            };

            UserForm reg = new UserForm()
            {
                UserName = model.UserName,
                Password = model.Password
            };

            string url = @"https://localhost:44380/api/application/users";
            var result = await httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8,
                mediaType: "application/json")
                );

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Error", "Users");
            }
            return RedirectToAction("Index");
        }

        //страница редактирования пользователя
        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {
                string url = @$"https://localhost:44380/api/application/users/{id}";
                string json = httpClient.GetStringAsync(url).Result;
                return View(JsonConvert.DeserializeObject<EditUserViewModel>(json));
            }
            return NotFound();
        }

        //редактирование пользователя
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            User user = new User()
            {
                Id = model.Id,
                UserName = model.UserName,
            };

            string url = @"https://localhost:44380/api/application/users";
            var result = await httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                );
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Error", "Users");
            }
            return RedirectToAction("Index");
        }

        //удаление пользователя
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            string url = @$"https://localhost:44380/api/application/users/{id}";
            var result = await httpClient.DeleteAsync(url);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Error", "Users");
            }
            return RedirectToAction("Index");
        }

        //страница ошибки
        public IActionResult Error()
        {
            return View();
        }
    }
}
