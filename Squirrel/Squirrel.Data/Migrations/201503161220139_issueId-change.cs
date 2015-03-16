namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class issueIdchange : DbMigration
    {
        public override void Up()
        {
            DropIndex("Blog.Topics", new[] { "IssueId" });
            AlterColumn("Blog.Topics", "IssueId", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("Blog.Topics", "IssueId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("Blog.Topics", new[] { "IssueId" });
            AlterColumn("Blog.Topics", "IssueId", c => c.String(maxLength: 15));
            CreateIndex("Blog.Topics", "IssueId");
        }
    }
}
