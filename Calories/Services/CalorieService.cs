using Calories.Database;
using Calories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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


        /// <summary>
        /// Returns a summary of what's been eaten today
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Meal>> GetTodaysMealsAsync(Person who)
        {
            var part1 = await _db.Meals
                .Include(m => m.Food)
                .Where(m => m.PersonID == who.ID)
                .Where(m => m.Timestamp >= Today)
                .ToListAsync();

            return part1.GroupBy(m => m.Food.ID)
                .Select(g => new Meal
                {
                    Count = g.Sum(m => m.Count),
                    Food = _db.Foods.Find(g.Key),
                });
        }

        public async Task<bool> DeleteMealAsync(Person who, Food toDelete)
        {
            Meal meal = await _db.Meals
                .Include(m => m.Food)
                .Where(m => m.PersonID == who.ID && m.Timestamp >= Today && m.Food.ID == toDelete.ID)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();

            if (meal == null)
            {
                return false;
            }


            if (meal.Count > 1)
            {
                meal.Count -= 1;
            }
            else
            {
                who.Meals.Remove(meal);
            }

            await _db.SaveChangesAsync();

            return true;
        }

        private DateTimeOffset Today {
            get {
                DateTimeOffset today = DateTimeOffset.Now;
                return new DateTimeOffset(today.Year, today.Month, today.Hour, 0, 0, 0, today.Offset);
            }
        }

        public IEnumerable<Meal> GetMeals()
        {
            return _db.Meals;
        }

        public async Task<bool> FoodExistsAsync(string name)
        {
            return await _db.Foods.AnyAsync(f => f.Name == name);
        }

        public IEnumerable<Food> GetFoods()
        {
            return _db.Foods;
        }

        public async Task<Food> AddFoodAsync(Person who, string name, double calories, Unit unit)
        {
            Food model = new Food
            {
                Owner = who,
                Calories = calories,
                Name = name,
                Unit = unit,
            };

            _db.Foods.Add(model);

            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<Meal> AddMealAsync(Person who, Food food, long count)
        {
            Meal meal = new Meal
            {
                Food = food,
                Person = who,
                Count = count,
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

        public async Task EditFoodAsync(Food current, string name, double calories, Unit unit)
        {
            current.Name = name;
            current.Calories = calories;
            current.Unit = unit;

            await _db.SaveChangesAsync();
        }

        public async Task<Meal> GetMealAsync(long id)
        {
            return await _db.Meals.FindAsync(id);
        }

        internal async Task DeleteMeal(Meal meal)
        {
            _db.Meals.Remove(meal);
            await _db.SaveChangesAsync();
        }
    }
}
