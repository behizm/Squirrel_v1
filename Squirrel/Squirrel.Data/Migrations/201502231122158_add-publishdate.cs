namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpublishdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Posts", "PublishDate", c => c.DateTime());
            AddColumn("Blog.Topics", "PublishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("Blog.Topics", "PublishDate");
            DropColumn("Blog.Posts", "PublishDate");
        }
    }
}
