using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using HangfireWeb.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWeb
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
            services.AddHangfire(configuration => configuration
                                   .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                   .UseSimpleAssemblyNameTypeSerializer()
                                   .UseRecommendedSerializerSettings()
                                   .UseSqlServerStorage
                                   (Configuration.GetConnectionString("DefaultConnection"),
                                   new SqlServerStorageOptions
                                   {
                                       CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                       SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                       QueuePollInterval = TimeSpan.Zero,
                                       UseRecommendedIsolationLevel = true,
                                       DisableGlobalLocks = true
                                   }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            //,             UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //new code:
            app.UseHangfireDashboard("/dashboard", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },

                //IsReadOnlyFunc = (DashboardContext context) => true

            });
            
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                
                endpoints.MapHangfireDashboard();

            });
        }
    }
}
