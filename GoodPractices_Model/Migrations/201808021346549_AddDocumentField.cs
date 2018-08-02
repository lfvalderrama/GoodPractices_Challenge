namespace GoodPractices_Challenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Document", c => c.String());
            AddColumn("dbo.Teachers", "Document", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "Document");
            DropColumn("dbo.Students", "Document");
        }
    }
}
