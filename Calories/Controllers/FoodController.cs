using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Lib;
using Calories.Models;
using Calories.Services;
using Calories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Calories.Controllers
{
    [Authorize(Roles = Static.RoleFood)]
    public class FoodController : BaseController
    {
        private readonly AuthService _auth;
        private readonly CalorieService _calories;

        public FoodController(AuthService auth, CalorieService calories)
        {
            _auth = auth;
            _calories = calories;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(_calories.GetFoods());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name", "Calories", "Unit")] FoodVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Person who = await _auth.GetCurrentPersonAsync(User);

            Food created = await _calories.AddFoodAsync(who, model.Name, model.Calories, model.Unit);

            return RedirectWithMessage(nameof(Index), "Food {0} was created", created.Name);
        }
    }
}
