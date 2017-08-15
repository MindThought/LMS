namespace LMS.Migrations
{
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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

			/////////////////////////////////
			// Course and courseComponents //
			/////////////////////////////////
			context.Courses.AddOrUpdate(
				c => c.Name,
				new Course
				{
					Id = 1,
					Name = ".Net2017",
					StartDate = new DateTime(2017, 4, 18),
					Description = ".NET för de med tidigare IT-erfarenhet"
				}
				,
				new Course
				{
					Id = 2,
					Name = "JAVA",
					StartDate = new DateTime(2017, 6, 18),
					Description = "JAVA för de med tidigare IT-erfarenhet"
				},
				new Course
				{
					Id = 3,
					Name = "Projektledning",
					StartDate = new DateTime(2017, 8, 18),
					Description = "Projektledning"
				},
				new Course
				{
					Id = 4,
					Name = ".NET-utbildning NB17",
					StartDate = new DateTime(2017, 4, 18),
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av programering"
				},
				new Course
				{
					Id = 5,
					Name = ".NET-utbildning NB18",
					StartDate = new DateTime(2017, 10, 18),
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av C++ programering"
				},
				new Course
				{
					Id = 6,
					Name = ".NET-utbildning NB20",
					StartDate = new DateTime(2018, 01, 18),
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av Delfi programering"
				},
				new Course
				{
					Id = 7,
					Name = ".NET-utbildning NB66",
					StartDate = new DateTime(2018, 03, 18),
					Description = "Systemutvecklare .NET för dem som har ingen erfarenhet av programering"
				}
				);

			context.SaveChanges();

			context.Modules.AddOrUpdate(
				m => m.Name,
				new Module
				{
					Name = "C#Default",
					CourseId = context.Courses.Where(c => c.Name == ".Net2017").FirstOrDefault().Id,
					StartDate = new DateTime(2017, 4, 19),
					Description = "Grundläggande C#",
					EndDate = new DateTime(2017, 5, 4)
				},
				new Module
				{
					Name = "JQuery",
					CourseId = context.Courses.Where(c => c.Name == ".Net2017").FirstOrDefault().Id,
					StartDate = new DateTime(2017, 5, 5),
					Description = "Grundläggande JQuery",
					EndDate = new DateTime(2017, 5, 30)
				},
				new Module
				{
					Name = "JAVA",
					Course = context.Courses.Where(c => c.Name == "JAVA").FirstOrDefault(),
					StartDate = new DateTime(2017, 6, 20),
					Description = "Grundläggande JQuery",
					EndDate = new DateTime(2017, 6, 30)
				},
				new Module
				{
					Name = "C#",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 4, 18),
					Description = "E-learning, forelesningar och övningsuppgifter: Grund, OOP, Generics och LINQ",
					EndDate = new DateTime(2017, 5, 16)
				},
				new Module
				{
					Name = "Webb",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 5, 17),
					Description = "E-learning, forelesningar och övningsuppgifter: HTML, CSS, JS, BOOTSTRAP och GIT",
					EndDate = new DateTime(2017, 5, 31)
				},
				new Module
				{
					Name = "MVC",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 6, 1),
					Description = "E-learning, forelesningar och övningsuppgifter: MVC, ASP.NET",
					EndDate = new DateTime(2017, 6, 15)
				},
				new Module
				{
					Name = "Databas",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 6, 16),
					Description = "E-learning, föreläsningar och övningsuppgifter: SQLBolt",
					EndDate = new DateTime(2017, 6, 22)
				},
				new Module
				{
					Name = "ApplikationsUtveckling",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 6, 26),
					Description = "E-learning, föreläsningar och övningsuppgifter: JS-Ramverk, Client vs. Server, UX och Identity",
					EndDate = new DateTime(2017, 6, 30)
				},
				new Module
				{
					Name = "Testning",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 7, 3),
					Description = "E-learning, föreläsningar och övningsuppgifter: Grundläggande Testning",
					EndDate = new DateTime(2017, 7, 7)
				},
				new Module
				{
					Name = "MVC fördjupning",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					StartDate = new DateTime(2017, 7, 21),
					Description = "E-learning, föreläsningar och övningsuppgifter: MVC, SKRUM, Projektplanering, " +
								  "Planering sprint (1-3), Sprint review och Slutredovisning (OBS! vv 30&31 Sommarstängt)",
					EndDate = new DateTime(2017, 8, 25)
				},


				new Module
				{
					Name = "Projektledning",
					Course = context.Courses.Where(c => c.Name == "Projektledning").FirstOrDefault(),
					StartDate = new DateTime(2017, 9, 5),
					Description = "Grundläggande JQuery",
					EndDate = new DateTime(2017, 9, 30)
				}
				);

			context.SaveChanges();

            context.Activities.AddOrUpdate(
                a => a.Name,
                new Activity
                {
                    Name = "C# Basics",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 8, 19, 8, 0, 0),
                    EndTime = new DateTime(2017, 8, 19, 12, 0, 0),
                    Description = "The basics of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },
                new Activity
                {
                    Name = "C# Intermediate",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 8, 19, 13, 0, 0),
                    EndTime = new DateTime(2017, 8, 19, 20, 0, 0),
                    Description = "The intermediates of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },

                new Activity
                {
                    Name = "test",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 9, 19, 13, 0, 0),
                    EndTime = new DateTime(2017, 9, 19, 20, 0, 0),
                    Description = "The intermediates of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },
                new Activity
                {
                    Name = "test 2",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 9, 20, 13, 0, 0),
                    EndTime = new DateTime(2017, 9, 21, 20, 0, 0),
                    Description = "The intermediates of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },
                new Activity
                {
                    Name = "C# Basics",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.Lecture,
                    StartTime = new DateTime(2017, 4, 20, 8, 0, 0),
                    EndTime = new DateTime(2017, 04, 20, 20, 0, 0),
                    Description = "Lectire with follow up coding"
                },
                new Activity
                {
                    Name = "C# Intermediate",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 4, 21, 08, 0, 0),
                    EndTime = new DateTime(2017, 04, 24, 20, 0, 0),
                    Description = "The intermediates of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },
                new Activity
                {
                    Name = "C# Intermediate",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.Lecture,
                    StartTime = new DateTime(2017, 4, 27, 8, 0, 0),
                    EndTime = new DateTime(2017, 04, 27, 20, 0, 0),
                    Description = "Lectire with follow up coding"
                },
                new Activity
                {
                    Name = "C# Exercise",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.Submission,
                    StartTime = new DateTime(2017, 4, 28, 8, 0, 0),
                    EndTime = new DateTime(2017, 05, 04, 20, 0, 0),
                    Description = "Individual code task. Console application: Garage 1.0"
                },
                new Activity
                {
                    Name = "C# Basics",
                    Module = context.Modules.Where(m => m.Name == "Projektledning").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 4, 19, 8, 0, 0),
                    EndTime = new DateTime(2017, 04, 19, 12, 0, 0),
                    Description = "The basics of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                }
                );

			context.SaveChanges();
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

			var teacherEmails = new[] { "john@lexicon.se", "dimitris@lexicon.se", "oscar@lexicon.se" };
			foreach (var email in teacherEmails)
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

			var studentEmails = new[] { "student0@lexicon.se", "student1@lexicon.se", "student2@lexicon.se", "student3@lexicon.se",
								 "student4@lexicon.se", "student5@lexicon.se", "student6@lexicon.se", "student7@lexicon.se" };
			foreach (var email in studentEmails)
			{
				if (!context.Users.Any(u => u.UserName == email))
				{
					var user = new ApplicationUser { UserName = email, Email = email, CourseId = context.Courses.Where(c => c.Name == ".Net2017").FirstOrDefault().Id };
					var result = userManager.Create(user, "foobar");
					if (!result.Succeeded)
					{
						throw new Exception(string.Join("\n", result.Errors));
					}
				}
			}

			foreach (var email in teacherEmails)
			{
				var teacherUser = userManager.FindByName(email);
				userManager.AddToRole(teacherUser.Id, "Teacher");
			}

			foreach (var email in studentEmails)
			{
				var studentUser = userManager.FindByName(email);
				userManager.AddToRole(studentUser.Id, "Student");
			}

			context.SaveChanges();
		}
	}
}
