using Calories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Database
{
    public class AuthService
    {


        public bool Authenticate(string Username, string Password)
        {
            return Username == Password;
        } 
    }
}
