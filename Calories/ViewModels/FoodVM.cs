using Calories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.ViewModels
{
    public class FoodVM : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Calories { get; set; }

        [Required]
        public Unit Unit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Calories <= 0)
            {
                yield return new ValidationResult("Must have some calories", new string[] { nameof(Calories) });
            }

            if (Unit == Unit.Unknown)
            {
                yield return new ValidationResult("Must choose a unit", new string[] { nameof(Unit) });
            }
        }
    }
}
