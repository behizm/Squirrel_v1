namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCategory1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Categories", "Description", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("Blog.Categories", "Description");
        }
    }
}
