using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Models
{
    public class MealFood
    {
        public long FoodID { get; set; }
        public virtual Food Food { get; set; }

        public long MealID { get; set; }
        public virtual Meal Meal { get; set; }
    }
}
