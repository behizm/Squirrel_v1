namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Blog.Posts", "Category_Id", "Blog.Categories");
            DropIndex("Blog.Posts", new[] { "Category_Id" });
            DropColumn("Blog.Posts", "Category_Id");
        }
        
        public override void Down()
        {
            AddColumn("Blog.Posts", "Category_Id", c => c.Guid());
            CreateIndex("Blog.Posts", "Category_Id");
            AddForeignKey("Blog.Posts", "Category_Id", "Blog.Categories", "Id");
        }
    }
}
