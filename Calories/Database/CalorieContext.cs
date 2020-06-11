using Calories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Database
{
    public class CalorieContext : DbContext
    {
        public CalorieContext(DbContextOptions<CalorieContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<MealFood> MealFoods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("People");
            modelBuilder.Entity<Meal>().ToTable("Meals");
            modelBuilder.Entity<Food>().ToTable("Foods");
            modelBuilder.Entity<MealFood>().ToTable("MealFoods");
        }
    }
}
