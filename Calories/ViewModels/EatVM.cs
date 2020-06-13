using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.ViewModels
{
    public class EatVM
    {
        [Required]
        public long ID { get; set; }

        public long Count { get; set; }
    }
}
