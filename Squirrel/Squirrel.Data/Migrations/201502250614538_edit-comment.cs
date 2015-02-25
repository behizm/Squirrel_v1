namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editcomment : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Comments", "IsReaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Blog.Comments", "IsReaded");
        }
    }
}
