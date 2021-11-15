using Hangfire;
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

    }
}
