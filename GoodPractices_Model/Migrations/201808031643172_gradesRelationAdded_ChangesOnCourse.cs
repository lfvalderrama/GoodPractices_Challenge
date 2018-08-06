namespace GoodPractices_Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gradesRelationAdded_ChangesOnCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Name", c => c.String());
            AddColumn("dbo.Grades", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Grades", "Subject_Id", c => c.Int());
            AddColumn("dbo.Grades", "Student_Id", c => c.Long());
            CreateIndex("dbo.Grades", "Subject_Id");
            CreateIndex("dbo.Grades", "Student_Id");
            AddForeignKey("dbo.Grades", "Subject_Id", "dbo.Subjects", "Id");
            AddForeignKey("dbo.Grades", "Student_Id", "dbo.Students", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grades", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Grades", "Subject_Id", "dbo.Subjects");
            DropIndex("dbo.Grades", new[] { "Student_Id" });
            DropIndex("dbo.Grades", new[] { "Subject_Id" });
            DropColumn("dbo.Grades", "Student_Id");
            DropColumn("dbo.Grades", "Subject_Id");
            DropColumn("dbo.Grades", "Type");
            DropColumn("dbo.Courses", "Name");
        }
    }
}
