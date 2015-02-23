namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editlog1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Site.Errors", "LineNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Site.Errors", "LineNumber");
        }
    }
}
