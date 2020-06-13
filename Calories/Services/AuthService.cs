using Calories.Database;
using Calories.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Calories.Services
{
    public class AuthService
    {
        private readonly CalorieContext _db;

        public AuthService(CalorieContext DB)
        {
            _db = DB;
        }

        public async Task<Person> AuthenticateAsync(string username, string password)
        {
            Person who = await GetPersonAsync(username);

            if (who != null && who.Password.Validate(password))
            {
                return who;
            }
            return null;
        }

        public async Task CreateUserAsync(string username, string password)
        {
            Password passwd = Password.Generate(password);
            _db.People.Add(new Person { Name = username, Password = passwd });
            await _db.SaveChangesAsync();
        }

        public async Task<Person> GetPersonAsync(string name)
        {
            return await _db.People
                .Include(p => p.Password)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task LoginFromWebAsync(HttpContext context, Person who)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, who.Name),
                        new Claim(ClaimTypes.Role, "User"),
                    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                IsPersistent = true,
                IssuedUtc = DateTimeOffset.Now,

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


        }

        public async Task<Person> GetCurrentPersonAsync(ClaimsPrincipal User)
        {

            return await GetPersonAsync(User.FindFirstValue(ClaimTypes.Name));

        }
    }
}
