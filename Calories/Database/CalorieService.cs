using Calories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Calories.Database
{
    public class CalorieService
    {
        private readonly CalorieContext _db;

        public CalorieService(CalorieContext DB)
        {
            _db = DB;
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

        public async Task<Food> GetFoodAsync(long id)
        {
            return await _db.Foods.FindAsync(id);
        }
    }
}
