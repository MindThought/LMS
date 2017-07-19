namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "Course_Id" });
            AlterColumn("dbo.Modules", "Name", c => c.String(maxLength: 60));
            AlterColumn("dbo.Modules", "Course_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Modules", "Course_Id");
            AddForeignKey("dbo.Modules", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "Course_Id" });
            AlterColumn("dbo.Modules", "Course_Id", c => c.Int());
            AlterColumn("dbo.Modules", "Name", c => c.String());
            CreateIndex("dbo.Modules", "Course_Id");
            AddForeignKey("dbo.Modules", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
