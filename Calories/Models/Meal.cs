using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Models
{
    public class Meal
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        [Required]
        public virtual Food Food { get; set; }
        public long FoodID { get; set; }

        [Required]
        public virtual Person Person { get; set; }
        public long PersonID { get; set; }
    }
}
