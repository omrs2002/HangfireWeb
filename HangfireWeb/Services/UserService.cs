using Hangfire;
using HangfireWeb.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWeb.Services
{
    public class UserService
    {
            
        public bool CreateUser()
        {
            Console.WriteLine("user Created");
            return true;
        }

        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public bool CreateUserWithFailed()
        {
            throw new Exception("Failed to create user!");
        }

        //private async Task CreateRolesandUsers()
        //{
            

        //    //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    //string[] roleNames = { "Admin", "Manager", "Member" };


        //    //bool x = await _roleManager.RoleExistsAsync("Admin");
        //    //if (!x)
        //    //{
        //    //    // first we create Admin rool    
        //    //    var role = new IdentityRole();
        //    //    role.Name = "Admin";
        //    //    await _roleManager.CreateAsync(role);

        //    //    //Here we create a Admin super user who will maintain the website                   

        //    //    var user = new ApplicationUser();
        //    //    user.UserName = "default";
        //    //    user.Email = "default@default.com";

        //    //    string userPWD = "somepassword";

        //    //    IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

        //    //    //Add default User to Role Admin    
        //    //    if (chkUser.Succeeded)
        //    //    {
        //    //        var result1 = await _userManager.AddToRoleAsync(user, "Admin");
        //    //    }
        //    //}

        //    //// creating Creating Manager role     
        //    //x = await _roleManager.RoleExistsAsync("Manager");
        //    //if (!x)
        //    //{
        //    //    var role = new IdentityRole();
        //    //    role.Name = "Manager";
        //    //    await _roleManager.CreateAsync(role);
        //    //}

        //    //// creating Creating Employee role     
        //    //x = await _roleManager.RoleExistsAsync("Employee");
        //    //if (!x)
        //    //{
        //    //    var role = new IdentityRole();
        //    //    role.Name = "Employee";
        //    //    await _roleManager.CreateAsync(role);
        //    //}
        //}


    }
}
