using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Calories.Database;
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
            if (ModelState.IsValid && _auth.Authenticate(model.Username, model.Password))
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,

                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.Now,

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                Message("Welcome back {0}", model.Username);

                if (string.IsNullOrEmpty(model.Next))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(model.Next);
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
