namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("Site.Logs", "IsAjax", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Site.Logs", "IsAjax");
        }
    }
}
