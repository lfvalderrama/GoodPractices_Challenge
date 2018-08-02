namespace GoodPractices_Challenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoursesOnSubject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Subjects", new[] { "Course_Id" });
            CreateTable(
                "dbo.SubjectCourses",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Course_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Course_Id);
            
            DropColumn("dbo.Subjects", "Course_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subjects", "Course_Id", c => c.Int());
            DropForeignKey("dbo.SubjectCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.SubjectCourses", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.SubjectCourses", new[] { "Course_Id" });
            DropIndex("dbo.SubjectCourses", new[] { "Subject_Id" });
            DropTable("dbo.SubjectCourses");
            CreateIndex("dbo.Subjects", "Course_Id");
            AddForeignKey("dbo.Subjects", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
