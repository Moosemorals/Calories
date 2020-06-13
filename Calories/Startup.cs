using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Calories.Database;
using Calories.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calories
{
    public class Startup
    {

        private readonly IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CalorieContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/Denied";
                    options.ReturnUrlParameter = "Next";
                    options.SlidingExpiration = true;
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                });

            services.AddScoped<CustomCookieAuthenticationEvents>();
            services.AddScoped<CalorieContext>();
            services.AddTransient<CalorieService>();
            services.AddTransient<AuthService>();

            if (_env.IsDevelopment())
            {
                services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
            } else
            {
                services.AddControllersWithViews();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
