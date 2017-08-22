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
					Name = ".Net2017",
					Description = ".NET för de med tidigare IT-erfarenhet"
				}
				,
				new Course
				{
					Name = "JAVA",
					Description = "JAVA för de med tidigare IT-erfarenhet"
				},
				new Course
				{
					Name = "Projektledning",
					Description = "Projektledning"
				},
				new Course
				{
					Name = ".NET-utbildning NB17",
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av programering"
				},
				new Course
				{
					Name = ".NET-utbildning NB18",
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av C++ programering"
				},
				new Course
				{
					Name = ".NET-utbildning NB20",
					Description = "Systemutvecklare .NET för dem som har tidigare erfarenhet av Delfi programering"
				},
				new Course
				{
					Name = ".NET-utbildning NB66",
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
					Description = "Grundläggande C#",
				},
				new Module
				{
					Name = "JQuery",
					CourseId = context.Courses.Where(c => c.Name == ".Net2017").FirstOrDefault().Id,
					Description = "Grundläggande JQuery",
				},
				new Module
				{
					Name = "JAVA",
					Course = context.Courses.Where(c => c.Name == "JAVA").FirstOrDefault(),
					Description = "Grundläggande JQuery",
				},
				new Module
				{
					Name = "C#",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, forelesningar och övningsuppgifter: Grund, OOP, Generics och LINQ",
				},
				new Module
				{
					Name = "Webb",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, forelesningar och övningsuppgifter: HTML, CSS, JS, BOOTSTRAP och GIT",
				},
				new Module
				{
					Name = "MVC",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, forelesningar och övningsuppgifter: MVC, ASP.NET",
				},
				new Module
				{
					Name = "Databas",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, föreläsningar och övningsuppgifter: SQLBolt",
				},
				new Module
				{
					Name = "ApplikationsUtveckling",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, föreläsningar och övningsuppgifter: JS-Ramverk, Client vs. Server, UX och Identity",
				},
				new Module
				{
					Name = "Testning",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, föreläsningar och övningsuppgifter: Grundläggande Testning",
				},
				new Module
				{
					Name = "MVC fördjupning",
					Course = context.Courses.Where(c => c.Name == ".NET-utbildning NB17").FirstOrDefault(),
					Description = "E-learning, föreläsningar och övningsuppgifter: MVC, SKRUM, Projektplanering, " +
								  "Planering sprint (1-3), Sprint review och Slutredovisning (OBS! vv 30&31 Sommarstängt)",
				},
				new Module
				{
					Name = "Projektledning",
					Course = context.Courses.Where(c => c.Name == "Projektledning").FirstOrDefault(),
					Description = "Grundläggande JQuery",
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
                    StartTime = new DateTime(2017, 8, 18, 8, 0, 0),
                    EndTime = new DateTime(2017, 8, 18, 12, 0, 0),
                    Description = "The basics of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>"
                },
                new Activity
                {
                    Name = "C# Intermediate",
                    Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
                    Type = Models.ActivityType.ELearning,
                    StartTime = new DateTime(2017, 8, 18, 13, 0, 0),
                    EndTime = new DateTime(2017, 8, 18, 20, 0, 0),
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
                    Type = Models.ActivityType.Lektion,
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
					Type = Models.ActivityType.Lektion,
					StartTime = new DateTime(2017, 4, 27, 8, 0, 0),
					EndTime = new DateTime(2017, 04, 27, 20, 0, 0),
					Description = "Lectire with follow up coding"
				},
				new Activity
				{
					Name = "C# Exercise",
					Module = context.Modules.Where(m => m.Name == "C#").FirstOrDefault(),
					Type = Models.ActivityType.Inlämning,
					StartTime = new DateTime(2017, 4, 28, 8, 0, 0),
					EndTime = new DateTime(2017, 05, 04, 20, 0, 0),
					Description = "Individual code task. Console application: Garage 1.0"
				},
				new Activity
				{
					Name = "Slutprojekt",
					Module = context.Modules.Where(m => m.Name == "MVC fördjupning").FirstOrDefault(),
					Type = Models.ActivityType.Inlämning,
					StartTime = new DateTime(2017, 8, 1, 8, 0, 0),
					EndTime = new DateTime(2017, 8, 24, 20, 0, 0),
					Description = "Individual code task. Console application: Garage 1.0"
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
			var teacherNames = new[] { "John", "Dimitris", "Oscar" };
			for (int i = 0; i < teacherEmails.Count(); i++)
			{
				var compareMail = teacherEmails[i];
				if (!context.Users.Any(u => u.UserName == compareMail))
				{
					var user = new ApplicationUser { UserName = teacherEmails[i], Email = teacherEmails[i], Name = teacherNames[i] };
					var result = userManager.Create(user, "foobar");
					if (!result.Succeeded)
					{
						throw new Exception(string.Join("\n", result.Errors));
					}
				}
			}

			var studentEmails = new[] { "student0@lexicon.se", "student1@lexicon.se", "student2@lexicon.se", "student3@lexicon.se",
								 "student4@lexicon.se", "student5@lexicon.se", "student6@lexicon.se", "student7@lexicon.se" };
			var studentNames = new[] {"Alfa", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta" };
			for (int i = 0; i < studentEmails.Count(); i++)
			{
				var compareMail = studentEmails[i];
				if (!context.Users.Any(u => u.UserName == compareMail))
				{
					var user = new ApplicationUser { UserName = studentEmails[i], Email = studentEmails[i], Name = studentNames[i], CourseId = context.Courses.Where(c => c.Name == ".Net2017").FirstOrDefault().Id };
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
