using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Calories.Models
{
    public class Person
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual Password Password {get;set;}

        [Required]
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
