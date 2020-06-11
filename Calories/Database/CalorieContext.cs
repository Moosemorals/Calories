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

            modelBuilder.Entity<Meal>()
                .ToTable("Meals")
                .HasKey(m => m.ID);

            modelBuilder.Entity<Food>()
                .ToTable("Foods")
                .HasKey(f => f.ID);
 
            modelBuilder.Entity<Person>()
                .ToTable("People")
                .HasKey(p => p.ID);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Meals)
                .WithOne(m => m.Person)
                .HasForeignKey(m => m.PersonID);
 

            modelBuilder.Entity<MealFood>().ToTable("MealFoods")
                .HasKey(mf => new { mf.FoodID, mf.MealID });

            modelBuilder.Entity<MealFood>()
                .HasOne(mf => mf.Meal)
                .WithMany(m => m.MealFoods)
                .HasForeignKey(mf => mf.MealID);

            modelBuilder.Entity<MealFood>()
                .HasOne(mf => mf.Food)
                .WithMany(f => f.MealFoods)
                .HasForeignKey(mf => mf.FoodID);

        }
    }
}
