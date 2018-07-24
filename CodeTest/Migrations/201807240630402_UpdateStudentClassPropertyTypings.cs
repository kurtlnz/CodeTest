namespace CodeTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStudentClassPropertyTypings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentClasses", "Class_ClassId", c => c.Int());
            AddColumn("dbo.StudentClasses", "Student_StudentId", c => c.Int());
            CreateIndex("dbo.StudentClasses", "Class_ClassId");
            CreateIndex("dbo.StudentClasses", "Student_StudentId");
            AddForeignKey("dbo.StudentClasses", "Class_ClassId", "dbo.Classes", "ClassId");
            AddForeignKey("dbo.StudentClasses", "Student_StudentId", "dbo.Students", "StudentId");
            DropColumn("dbo.StudentClasses", "StudentId");
            DropColumn("dbo.StudentClasses", "ClassId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentClasses", "ClassId", c => c.Int(nullable: false));
            AddColumn("dbo.StudentClasses", "StudentId", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentClasses", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentClasses", "Class_ClassId", "dbo.Classes");
            DropIndex("dbo.StudentClasses", new[] { "Student_StudentId" });
            DropIndex("dbo.StudentClasses", new[] { "Class_ClassId" });
            DropColumn("dbo.StudentClasses", "Student_StudentId");
            DropColumn("dbo.StudentClasses", "Class_ClassId");
        }
    }
}
