using Calories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<Food> Foods { get; set; }

        public IEnumerable<Meal> Meals { get; set; }

        public Person Who { get; set; }
    }
}
