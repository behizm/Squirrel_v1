namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("Site.Configs", "Description", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("Site.Configs", "Description");
        }
    }
}
