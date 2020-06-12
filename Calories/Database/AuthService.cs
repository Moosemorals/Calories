using Calories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Database
{
    public class AuthService
    {
        private readonly CalorieContext _db;

        public AuthService(CalorieContext DB)
        {
            _db = DB;
        }

        public bool Authenticate(string Username, string Password)
        {
            return Username == Password;
        } 

        public async Task CreateUserAsync(string username, string password)
        {
            _db.People.Add(new Person { Name = username });
            await _db.SaveChangesAsync();
        }
    }
}
