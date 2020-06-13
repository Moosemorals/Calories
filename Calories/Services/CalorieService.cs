using Calories.Database;
using Calories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Calories.Services
{
    public class CalorieService
    {
        private readonly CalorieContext _db;

        public CalorieService(CalorieContext DB)
        {
            _db = DB;
        }

        public IEnumerable<Meal> GetMeals(Person who)
        {
            return _db.Meals
                .Include(m => m.Food)
                .Where(m => m.PersonID == who.ID);
        }

        public IEnumerable<Meal> GetMeals()
        {
            return _db.Meals;
        }

        public IEnumerable<Food> GetFoods()
        {
            return _db.Foods;
        }

        public async Task<Food> AddFoodAsync(string Name, long calories, Unit unit)
        {
            Food model = new Food
            {
                Calories = calories,
                Name = Name,
                Unit = unit,
            };
            _db.Foods.Add(model);

            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<Meal> AddMealAsync(Person who, Food food)
        {
            Meal meal = new Meal
            {
                Food = food,
                Person = who,
                Timestamp = DateTimeOffset.Now,
            };

            if (who.Meals == null)
            {
                who.Meals = new List<Meal>();
            }

            who.Meals.Add(meal);

            await _db.SaveChangesAsync();

            return meal;
        }

        public async Task<Food> GetFoodAsync(long id)
        {
            return await _db.Foods.FindAsync(id);
        }
        public async Task<Person> GetPersonAsync(long id)
        {
            return await _db.People.FindAsync(id);
        }
    }
}
