using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Models;
using Calories.Services;
using Calories.ViewModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Calories.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : BaseController
    {

        private readonly CalorieService _calories;
        private readonly AuthService _auth;

        public HomeController(AuthService auth, CalorieService calories)
        {
            _auth = auth;
            _calories = calories;
        }

        public async Task<IActionResult> IndexAsync()
        {

            Person who = await _auth.GetCurrentPersonAsync(User);
            return View(new IndexVM
            {
                Foods = _calories.GetFoods(),
                Meals = _calories.GetMeals(who),
                Who = who,
            });

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Eat([Bind("ID")] EatVM model)
        {

            Food food = await _calories.GetFoodAsync(model.ID);
            if (food == null)
            {
                return RedirectWithMessage("Index", "Can't find requested food");
            }

            Person who = await _calories.GetPersonAsync(1); // TODO: People

            await _calories.AddMealAsync(who, food);

            return RedirectWithMessage("Index", "You have eaten {0}", food.Name);
        }
    }
}
