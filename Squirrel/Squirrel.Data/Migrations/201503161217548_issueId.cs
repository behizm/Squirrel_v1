namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issueId : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Topics", "IssueId", c => c.String(maxLength: 15));
            CreateIndex("Blog.Topics", "IssueId");
        }
        
        public override void Down()
        {
            DropIndex("Blog.Topics", new[] { "IssueId" });
            DropColumn("Blog.Topics", "IssueId");
        }
    }
}
