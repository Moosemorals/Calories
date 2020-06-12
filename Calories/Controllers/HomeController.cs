using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Database;
using Calories.Models;
using Calories.ViewModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : BaseController
    {

        private readonly CalorieService _db;

        public HomeController(CalorieService DB)
        {
            _db = DB;
        }

        public async Task<IActionResult> IndexAsync()
        {

            Person who = await _db.GetPersonAsync(1); // TODO: People
            return View(new IndexVM
            {
                Foods = _db.GetFoods(),
                Meals = _db.GetMeals(who),
                Who = who,
            });

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Eat([Bind("ID")] EatVM model)
        {

            Food food = await _db.GetFoodAsync(model.ID);
            if (food == null)
            {
                return RedirectWithMessage("Index", "Can't find requested food");
            }

            Person who = await _db.GetPersonAsync(1); // TODO: People

            await _db.AddMealAsync(who, food);

            return RedirectWithMessage("Index", "You have eaten {0}", food.Name);
        }
    }
}
