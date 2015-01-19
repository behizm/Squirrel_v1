namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTopic : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Topics", "PostsOrdering", c => c.Int(nullable: false));
            AddColumn("Blog.Topics", "IsPublished", c => c.Boolean(nullable: false));
            DropColumn("Blog.Topics", "FirstPost");
        }
        
        public override void Down()
        {
            AddColumn("Blog.Topics", "FirstPost", c => c.Int(nullable: false));
            DropColumn("Blog.Topics", "IsPublished");
            DropColumn("Blog.Topics", "PostsOrdering");
        }
    }
}
