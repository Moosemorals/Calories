using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Database;
using Calories.Models;
using Calories.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{
    public class HomeController : BaseController
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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Eat([Bind("ID")]EatVM model)
        {

            Food food = await _db.GetFoodAsync(model.ID);
            if (food == null)
            {
                return RedirectWithMessage("Index", "Can't find requested food");
            }


            return RedirectWithMessage("Index", "You have eaten {0}", food.Name);
        }
    }
}
