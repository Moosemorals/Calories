using Calories.Lib;
using Calories.Models;
using Calories.Services;
using Calories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Calories.Controllers
{
    [Authorize(Roles = Static.RoleFood)]
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
                Meals = _calories.GetMeals(who, TimeSpan.FromDays(1)),
                Who = who,
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Eat([Bind("ID, Count")] EatVM model)
        {
            if (model.Count == 0)
            {
                model.Count = 1;
            }

            Food food = await _calories.GetFoodAsync(model.ID);
            if (food == null)
            {
                return RedirectWithMessage("Index", "Can't find requested food");
            }

            Person who = await _auth.GetCurrentPersonAsync(User);

            await _calories.AddMealAsync(who, food, model.Count);

            return RedirectWithMessage("Index", "You have eaten {0} {1}{2}", model.Count, food.Name, model.Count == 1 ? "": "s");
        }
    }
}
