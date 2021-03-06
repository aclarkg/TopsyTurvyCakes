﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TopsyTurvyCakes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NJDOTNET
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()  // Lock down certain folders
                    .AddRazorPagesOptions(_ => {
                        _.Conventions.AuthorizeFolder("/Account");
                        _.Conventions.AuthorizeFolder("/Admin");
                        _.Conventions.AllowAnonymousToPage("/Account/Login"); // Override, allow any access to login page
                    });

            services.AddDbContext<RecipesDbContext>(options =>
            {
                options.UseInMemoryDatabase("TopsyTurvyCakes");
            });

            services.AddScoped<IRecipesService, RecipesService>();

            // Authenticate with cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            //Get route
            app.UseMvcWithDefaultRoute();
        }
    }
}
