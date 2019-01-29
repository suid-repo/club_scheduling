namespace WebApplication.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using WebApplication.Core;
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

            if (!roleManager.RoleExists("Member"))
            {
                roleManager.Create(
                    new IdentityRole
                    {
                        Name = "Member"
                    });
            }

            //SEED DEFAULT ADMIN USER IF NO ONE EXIST
            if (!context.Users.Any(u => u.Roles.All(r => r.RoleId == context.Roles.FirstOrDefault(m => m.Name == "Head Coach").Id)))
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

            //SEED LEVELS
            context.Levels.AddOrUpdate(
                new Level
                {
                    Id = 1,
                    Name = "Adults"
                },
                new Level
                {
                    Id = 2,
                    Name = "Advanced"
                },
                new Level
                {
                    Id = 3,
                    Name = "Improver"
                },
                new Level
                {
                    Id = 4,
                    Name = "Intermediate"
                },
                new Level
                {
                    Id = 5,
                    Name = "Kids"
                },
                new Level
                {
                    Id = 6,
                    Name = "Learner"
                });


            //SEED STORED PROCEDURE / TRIGGER
            SeedAdditionalTableStuff(context);
        }

        private void SeedAdditionalTableStuff(WebApplication.Models.ApplicationDbContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
             //   System.Diagnostics.Debugger.Launch();
            //Initialize File Reader
            var assembly = Assembly.GetExecutingAssembly();

            Console.Write(this.GetType().Assembly.GetManifestResourceNames());
            if (context.Database.SqlQuery<int>("IF TYPE_ID(N'QueuedListTableType') IS NULL SELECT 1 ELSE SELECT 0;").FirstOrDefault() == 1)
            {

                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.QueuedListTableType.sql"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                    }
            }

            if (context.Database.SqlQuery<int>("IF TYPE_ID(N'UserListTableType') IS NULL SELECT 1 ELSE SELECT 0;").FirstOrDefault() == 1)
            {

                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.UserListTableType.sql"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                    }
            }

            if (context.Database.SqlQuery<int>("IF TYPE_ID(N'FamilyQueuedListTableType') IS NULL SELECT 1 ELSE SELECT 0;").FirstOrDefault() == 1)
            {

                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.FamilyQueuedListTableType.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'P_Move2Event') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.P_Move2Event.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'P_Move2Queued') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.P_Move2Queued.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'P_CoachJoined') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.P_CoachJoined.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'P_CoachLeaved') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.P_CoachLeaved.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'P_CheckQueued') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.P_CheckQueued.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'T_CoachJoigned') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.T_CoachJoigned.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }

            if (context.Database.SqlQuery<int>("IF object_id(N'T_CoachLeaved') IS NULL SELECT 1 ELSE SELECT 0").FirstOrDefault() == 1)
            {
                using (Stream stream = assembly.GetManifestResourceStream("WebApplication.SqlSources.T_CoachLeaved.sql"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    context.Database.ExecuteSqlCommand(reader.ReadToEnd());
                }
            }
            
        }
    }
}
