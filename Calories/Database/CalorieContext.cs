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
        public CalorieContext() : base() { }

        public DbSet<Person> People { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite(@"Data Source=C:\temp\calories.sqlite;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Food>()
                .ToTable("Foods")
                .HasKey(f => f.ID);

            modelBuilder.Entity<Person>()
                .ToTable("People")
                .HasKey(p => p.ID);

            modelBuilder.Entity<Meal>()
               .ToTable("Meals")
               .HasKey(m => m.ID);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Meals)
                .WithOne(m => m.Person)
                .HasForeignKey(m => m.PersonID);

            modelBuilder.Entity<Person>()
                .HasData(new Person { ID = 1, Name = "Test" });

        }
    }
}
