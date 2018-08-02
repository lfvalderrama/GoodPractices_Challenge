namespace GoodPractices_Challenge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignlanguagechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Language", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Language");
        }
    }
}
