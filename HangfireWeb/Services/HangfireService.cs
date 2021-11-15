using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireWeb.Services
{
    public static class HangfireService
    {
        
        
        public static void CreateUserJob()
        {
            try
            {
                UserService usr = new();
                var jobID = BackgroundJob.Enqueue(() => usr.CreateUserWithFailed());
            }
            catch 
            {
            }

        }
    }
}
