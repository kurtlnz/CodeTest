namespace CodeTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        Location = c.String(),
                        TeacherName = c.String(),
                    })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.StudentClasses",
                c => new
                    {
                        StudentClassId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentClassId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        GivenName = c.String(),
                        Surname = c.String(maxLength: 255),
                        AgeInYears = c.Int(nullable: false),
                        GPA = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId)
                .Index(t => t.Surname, unique: true, name: "SurnameIndex");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", "SurnameIndex");
            DropTable("dbo.Students");
            DropTable("dbo.StudentClasses");
            DropTable("dbo.Classes");
        }
    }
}
