using Hangfire;
using HangfireWeb.Data;
using HangfireWeb.Models;
using HangfireWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        ApplicationDbContext _dbcontx;
        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext dbcontx,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _logger = logger;
            _dbcontx = dbcontx;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            CreateRolesandUsers();
            //HangfireService.CreateUserJob();
            return View();
        }

        public IActionResult Privacy()
        {
          
            return View();
        }

        private  void CreateRolesandUsers()
        {

           string[] roleNames = { "Admin", "Manager", "Member" };
            
            bool x =  _dbcontx.Roles.FirstOrDefault(r=>r.Name == "Admin" ) != null;
            if (!x)
            {
                // first we create Admin rool    
                List<IdentityRole> roles = new()
                {
                    new IdentityRole{ Name = "Admin", NormalizedName ="Admin" },
                    new IdentityRole{ Name = "Manager", NormalizedName ="Manager" },
                    new IdentityRole{ Name = "Member", NormalizedName ="Member" }
                };
                 _dbcontx.Roles.AddRange(roles);
                _dbcontx.SaveChanges();
                //Here we create a Admin super user who will maintain the website                   

                var user = new IdentityUser
                {
                    UserName = "default",
                    Email = "default@default.com",
                    EmailConfirmed = true
                };
                string userPWD = "User1234#";
                var result =  _userManager.CreateAsync(user, userPWD);
               
                    _logger.LogInformation("User created a new account with password.");
                    var result1 =  _userManager.AddToRoleAsync(user, "Admin");
                    var test = _userManager.IsInRoleAsync(user, "Admin");
                    if(test.Status == TaskStatus.Created)
                        _logger.LogInformation("User Added to rule.");

                
                
                 _dbcontx.SaveChangesAsync();

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
