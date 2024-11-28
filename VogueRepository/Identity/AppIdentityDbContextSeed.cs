using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VogueCore.Entities.Identity;

namespace VogueRepository.Identity
{
    public static class AppIdentityDbContextSeed
    {
       
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Ahmed Hamdi",
                    Email = "Ahmedhamdi026@gmail.com",
                    UserName = "Ahmedhamdi026",
                    PhoneNumber = "01280388341"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
          
        }
    }
}
