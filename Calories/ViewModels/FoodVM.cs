using Calories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.ViewModels
{
    public class FoodVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long Calories { get; set; }

        [Required]
        public Unit Unit { get; set; }

 
    }
}
