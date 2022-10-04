using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Options;
using WebQueue.Models;

namespace WebQueue.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebQueue.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        
        protected override void Seed(WebQueue.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var roleStore = new RoleStore<IdentityRole>();
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userManager = new ApplicationUserManager(userStore);
            
            var role = roleManager.FindByName("admin");
            var user = userManager.FindByEmail("admin@admin.com");

            if (role == null && user == null)
            {
                var appUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                };

                var appRole = new IdentityRole() {Name = "admin"};

                userManager.Create(appUser, "Admin123@");
                roleManager.Create(appRole);

                var newUser = userManager.FindByEmail("admin@admin.com");

                userManager.AddToRole(newUser.Id, "admin");
            }
        }
    }
}