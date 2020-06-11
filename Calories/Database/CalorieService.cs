using Calories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
