using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TheoryOfPoltaran.Models;
using TheoryOfPoltaran.ViewModels;

namespace TheoryOfPoltaran.Controllers
{
    public class AdminController : Controller
    {
        private MainContext _mainContext;
        public AdminController(MainContext mainContext)
        {
            _mainContext = mainContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var user = await _mainContext.Users.FirstOrDefaultAsync();
            if(user == null)
                return RedirectToAction("Register", "Admin");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var user = await _mainContext.Users.FirstOrDefaultAsync();
            if(user != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _mainContext.Users.FirstOrDefaultAsync();
                if (user != null)
                {
                    await Authenticate(model.Login); // аутентификация

                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _mainContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    _mainContext.Users.Add(new User { Login = model.Login, Password = model.Password });
                    await _mainContext.SaveChangesAsync();

                    await Authenticate(model.Login); // аутентификация

                    return RedirectToAction("Index", "Admin");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
