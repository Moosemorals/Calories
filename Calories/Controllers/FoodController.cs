using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calories.Models;
using Calories.Services;
using Calories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calories.Controllers
{

    [Authorize(Roles = "User")]
    public class FoodController : BaseController
    {

        private readonly CalorieService _db;

        public FoodController(CalorieService DB)
        {
            _db = DB;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(_db.GetFoods());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name", "Calories", "Unit")]FoodVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Food created = await _db.AddFoodAsync(model.Name, model.Calories, model.Unit);

            return RedirectWithMessage(nameof(Index), "Food {0} was created", created.Name);
        }
    }
}
