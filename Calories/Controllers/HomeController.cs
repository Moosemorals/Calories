using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Database;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{
    public class HomeController : Controller
    {

        private readonly CalorieService _db;

        public HomeController(CalorieService DB)
        {
            _db = DB;
        }

        public IActionResult Index()
        {
            return View(_db.GetFoods());
        }
    }
}
