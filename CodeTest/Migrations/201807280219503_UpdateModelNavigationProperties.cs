namespace CodeTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelNavigationProperties : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.StudentClasses", "StudentId");
            CreateIndex("dbo.StudentClasses", "ClassId");
            AddForeignKey("dbo.StudentClasses", "ClassId", "dbo.Classes", "ClassId", cascadeDelete: true);
            AddForeignKey("dbo.StudentClasses", "StudentId", "dbo.Students", "StudentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentClasses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentClasses", "ClassId", "dbo.Classes");
            DropIndex("dbo.StudentClasses", new[] { "ClassId" });
            DropIndex("dbo.StudentClasses", new[] { "StudentId" });
        }
    }
}
