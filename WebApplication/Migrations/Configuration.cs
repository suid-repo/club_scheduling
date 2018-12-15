namespace WebApplication.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            PasswordHasher ps = new PasswordHasher();

            //MANAGER
            var manager =
                new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            //SEED ROLES
            if (!roleManager.RoleExists("Head Coach"))
            {
                roleManager.Create(
                    new IdentityRole
                    {
                        Name = "Head Coach"
                    });
            }

            if (!roleManager.RoleExists("Coach"))
            {
                roleManager.Create(
                    new IdentityRole
                    {
                        Name = "Coach"
                    });
            }

            //SEED DEFAULT ADMIN USER IF NO ONE EXIST
            if (manager.FindByName("admin@localhost.com") == null)
            {
                ApplicationUser firstHeadCoach = new ApplicationUser
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com",
                    PasswordHash = ps.HashPassword("Admin$1"),
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (manager.Create(firstHeadCoach).Succeeded)
                {
                    manager.AddToRoles(firstHeadCoach.Id, new string[] { "Head Coach", "Coach" });
                }
            }
                
            
        }
    }
}
