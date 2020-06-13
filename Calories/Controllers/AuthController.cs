using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Calories.Models;
using Calories.Services;
using Calories.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthService _auth;

        public AuthController(AuthService Auth)
        {
            _auth = Auth;
        }

        [HttpGet]
        public IActionResult Login(string Next)
        {
            return View(new LoginVM { Next = Next });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync([Bind(nameof(LoginVM.Username), nameof(LoginVM.Password))] LoginVM model)
        {
            if (ModelState.IsValid)
            {
                Person who = await _auth.AuthenticateAsync(model.Username, model.Password);

                if (who != null)
                {
                    await _auth.LoginFromWebAsync(HttpContext, who);
                    Message("Welcome back {0}", who.Name);
                    if (string.IsNullOrEmpty(model.Next))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(model.Next);
                    } 
                } 
            }

            return View(model);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            Message("You have been logged out");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}
