﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Models
{
    public class Food
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long Calories { get; set; }

        [Required]
        public Unit Unit { get; set; }

        public virtual ICollection<MealFood> MealFoods { get; set; }
    }

    public enum Unit
    {
        Unknown,
        Each,
        Gram,
    }
}