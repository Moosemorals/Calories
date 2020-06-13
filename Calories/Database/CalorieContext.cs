﻿using Calories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calories.Database
{
    public class CalorieContext : DbContext
    {

        private readonly IConfiguration _config;

        public CalorieContext(IConfiguration Config) : base() {
            _config = Config;
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite(_config["ConnectionStrings:DefaultConnection"]);
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

            modelBuilder.Entity<Password>()
                .ToTable("Passwords")
                .HasKey(p => p.ID);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Password)
                .WithOne(p => p.Person)
                .HasForeignKey<Password>(p => p.PersonID);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Foods)
                .WithOne(f => f.Owner)
                .HasForeignKey(f => f.OwnerID);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Meals)
                .WithOne(m => m.Person)
                .HasForeignKey(m => m.PersonID);
        }
    }
}
