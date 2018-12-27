using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AdvertSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AdvertSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<advert_siteContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<advert_siteContext>();

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(app.ApplicationServices).Wait();

        }


        private async Task CreateRoles(IServiceProvider serviceProvider)
        {

                // Seed database code goes here

                //initializing custom roles 
                var RoleManager = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string[] roleNames = { "Admin", "User" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database: Question 1
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                //Here you could create a super user who will maintain the web app
                var poweruser = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                };
                //Ensure you have these values in your appsettings.json file
                string userPWD = "Admin1*";
                var _user = await UserManager.FindByEmailAsync("admin@gmail.com");

                if (_user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the role
                        await UserManager.AddToRoleAsync(poweruser, "Admin");

                    }
                }
            
        }
    }
}
