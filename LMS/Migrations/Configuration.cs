namespace LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Courses.AddOrUpdate(
                c => c.Name,
                new Course { Name = ".Net2017", StartDate = new DateTime(2017,4,18), Description = ".NET för de med tidigare IT-erfarenhet"}
                );
            context.Modules.AddOrUpdate(
                m => m.Name,
                new Module { Name = "C#", CourseId = 1 , StartDate = new DateTime(2017, 4, 19), Description = "Grundläggande C#", EndDate = new DateTime(2017, 5, 4) }
                );
            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity { Name = "C# Basics", Module = context.Modules.Find(1), Type = Models.ActivityType.ELearning, StartTime = new DateTime(2017, 4, 19, 8, 0, 0), EndTime = new DateTime(2017, 04, 19, 12, 0, 0), Description = "The basics of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>" }
                );


            ////////////////////////
            // User Seeding below //
            ////////////////////////
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    var result = roleManager.Create(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var emails = new[] { "john@lexicon.se", "dimitris@lexicon.se", "student@lexicon.se" };
            foreach (var email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var result = userManager.Create(user, "foobar");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            var johnUser = userManager.FindByName("john@lexicon.se");
            userManager.AddToRole(johnUser.Id, "Teacher");

            var dimitrisUser = userManager.FindByName("dimitris@lexicon.se");
            userManager.AddToRole(dimitrisUser.Id, "Teacher");

            var studentUser = userManager.FindByName("student@lexicon.se");
            userManager.AddToRole(studentUser.Id, "Student");

            context.SaveChanges();
        }
    }
}
