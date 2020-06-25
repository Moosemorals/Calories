using Calories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        public CalorieContext(IConfiguration Config) : base()
        {
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
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }


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
