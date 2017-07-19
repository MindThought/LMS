namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext context)
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
                new Models.Course { Name = ".Net2017", StartDate = new DateTime(2017,4,18), Description = ".NET för de med tidigare IT-erfarenhet"}
                );
            context.Modules.AddOrUpdate(
                m => m.Name,
                new Models.Module { Name = "C#", Course = context.Courses.Find(1) , StartDate = new DateTime(2017, 4, 19), Description = "Grundläggande C#", EndDate = new DateTime(2017, 5, 4) }
                );
            context.Activities.AddOrUpdate(
                a => a.Name,
                new Models.Activity { Name = "C# Basics", Module = context.Modules.Find(1), Type = Models.ActivityType.ELearning, StartTime = new DateTime(2017, 4, 19, 8, 0, 0), EndTime = new DateTime(2017, 04, 19, 12, 0, 0), Description = "The basics of C# on <a href= https://app.pluralsight.com/library/courses/c-sharp-fundamentals-with-visual-studio-2015 > course </a>" }
                );
        }
    }
}
