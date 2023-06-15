using m21_e2_WEB.Models;
using m21_e2_WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace m21_e2_WEB.Controllers
{
    public class AccountController : Controller //контроллер управления логином и регистрацией
    {        
        private readonly SignInManager<User> _signInManager;
        public HttpClient httpClient { get; set; }

        public AccountController(SignInManager<User> signInManager)
        {            
            _signInManager = signInManager;
            httpClient = new HttpClient();
        }
        [HttpGet]
        //страница регистрации
        public IActionResult Register()
        {
            return View();
        }

        //регистрация
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
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

                string url = @"https://localhost:44380/api/application/register";
                var result = await httpClient.PostAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8,
                    mediaType: "application/json")
                    );

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await _signInManager.SignInAsync(newUser, false);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return RedirectToAction("ErrorCreateAccount", "Account");
                }
            }
            return View(model);
        }
        
        //страница логина
        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        //логин
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserForm log = new UserForm()
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                User user = new User()
                {
                    UserName = log.UserName
                };

                string url = @"https://localhost:44380/api/application/login";
                var result = await httpClient.PostAsync(
                    requestUri: url,
                    content: new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8,
                    mediaType: "application/json")
                    );

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    return RedirectToAction("ErrorLoginAccount", "Account");
                }
            }
            return View(model);
        }

        //логаут
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //страница запрета доступа
        public IActionResult AccessDenied()
        {
            return View();
        }

        //страница ошибки при регистрации
        public IActionResult ErrorCreateAccount()
        {
            return View();
        }

        //страница ошибки при логине
        public IActionResult ErrorLoginAccount()
        {
            return View();
        }
    }
}
