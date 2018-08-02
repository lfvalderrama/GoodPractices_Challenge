namespace GoodPractices_Challenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToManyTeachrSubject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "Teacher_Id", "dbo.Teachers");
            DropIndex("dbo.Subjects", new[] { "Teacher_Id" });
            CreateTable(
                "dbo.SubjectTeachers",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Teacher_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Teacher_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Teacher_Id);
            
            DropColumn("dbo.Subjects", "Teacher_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subjects", "Teacher_Id", c => c.Long());
            DropForeignKey("dbo.SubjectTeachers", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.SubjectTeachers", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.SubjectTeachers", new[] { "Teacher_Id" });
            DropIndex("dbo.SubjectTeachers", new[] { "Subject_Id" });
            DropTable("dbo.SubjectTeachers");
            CreateIndex("dbo.Subjects", "Teacher_Id");
            AddForeignKey("dbo.Subjects", "Teacher_Id", "dbo.Teachers", "Id");
        }
    }
}
