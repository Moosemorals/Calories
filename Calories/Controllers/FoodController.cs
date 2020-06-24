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
            if (await _calories.FoodExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), $"Food {model.Name} alredy exists");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Person who = await _auth.GetCurrentPersonAsync(User);

            Food created = await _calories.AddFoodAsync(who, model.Name, model.Calories, model.Unit);

            return RedirectWithMessage(nameof(Index), "Food {0} was created", created.Name);
        }

        [HttpGet]
        public async Task<ActionResult> EditAsync(long? id)
        {
            if (id == null)
            {
                return RedirectWithMessage(nameof(Index), "Missing id");
            }
            Food current = await _calories.GetFoodAsync(id.Value);
            if (current == null)
            {
                return RedirectWithMessage(nameof(Index), "Can't find food with id {0}", id.Value);
            }

            return View(new FoodVM
            {
                Calories = current.Calories,
                Name = current.Name,
                Unit = current.Unit,
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(long? id, FoodVM model)
        {
            if (id == null)
            {
                return RedirectWithMessage(nameof(Index), "Missing id");
            }
            Food current = await _calories.GetFoodAsync(id.Value);
            if (current == null)
            {
                return RedirectWithMessage(nameof(Index), "Can't find food with id {0}", id.Value);
            }

            if (model.Name != current.Name && await _calories.FoodExistsAsync(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), $"Food {model.Name} alredy exists");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _calories.EditFoodAsync(current, model.Name, model.Calories, model.Unit);

            return RedirectWithMessage(nameof(Index), "Food {0} has been updated", current.Name);
        }
    }
}
