﻿using Calories.Database;
using Calories.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Security.Cryptography.Xml;

namespace Calories.Admin
{
    class Program
    {
        static int Main(string[] args)
        {

            RootCommand root = new RootCommand();

            Command addUser = new Command("adduser") {
                new Option<string>(new string[] {"--username", "-u" }, description: "The name of the user/account" ),
                new Option<string>(new string[] {"--password", "-p" }, description: "The password for the new user/account" ),
            };

            addUser.Handler = CommandHandler.Create<string, string>(async (username, password) =>
            {

                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile("secrets.json", true, true)
                    .Build();

                CalorieContext db = new CalorieContext(config);

                AuthService auth = new AuthService(db);

                await auth.CreateUserAsync(username, password);

                Console.WriteLine("User {0} created", username);

            });

            root.Add(addUser);

            return root.InvokeAsync(args).Result;

        }
    }
}
