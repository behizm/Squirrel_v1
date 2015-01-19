namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Blog.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Guid(),
                        AvatarId = c.Guid(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Blog.Files", t => t.AvatarId)
                .ForeignKey("Blog.Categories", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.AvatarId);
            
            CreateTable(
                "Blog.Files",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 25),
                        Address = c.String(nullable: false),
                        Filename = c.String(nullable: false, maxLength: 50),
                        Size = c.Int(nullable: false),
                        EditDate = c.DateTime(),
                        Type = c.Int(nullable: false),
                        Category = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Post_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Membership.Users", t => t.UserId)
                .ForeignKey("Blog.Posts", t => t.Post_Id)
                .Index(t => t.UserId)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "Membership.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        AccessFailed = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        EditeDate = c.DateTime(),
                        LastLogin = c.DateTime(),
                        IsLock = c.Boolean(nullable: false),
                        LockDate = c.DateTime(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Membership.Profiles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Firstname = c.String(nullable: false, maxLength: 30),
                        Lastname = c.String(nullable: false, maxLength: 50),
                        EditDate = c.DateTime(),
                        AvatarId = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("Blog.Files", t => t.AvatarId)
                .ForeignKey("Membership.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AvatarId);
            
            CreateTable(
                "Blog.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Abstract = c.String(maxLength: 300),
                        Body = c.String(nullable: false),
                        EditDate = c.DateTime(),
                        IsPublic = c.Boolean(nullable: false),
                        TopicId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        HeaderImageId = c.Guid(),
                        CreateDate = c.DateTime(nullable: false),
                        Category_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Blog.Files", t => t.HeaderImageId)
                .ForeignKey("Blog.Topics", t => t.TopicId)
                .ForeignKey("Membership.Users", t => t.UserId)
                .ForeignKey("Blog.Categories", t => t.Category_Id)
                .Index(t => t.TopicId)
                .Index(t => t.UserId)
                .Index(t => t.HeaderImageId)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "Blog.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Body = c.String(nullable: false),
                        PostId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Blog.Comments", t => t.ParentId)
                .ForeignKey("Blog.Posts", t => t.PostId)
                .Index(t => t.PostId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "Blog.Tags",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 25),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Blog.Topics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 150),
                        EditDate = c.DateTime(),
                        FirstPost = c.Int(nullable: false),
                        View = c.Int(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Blog.Categories", t => t.CategoryId)
                .ForeignKey("Membership.Users", t => t.UserId)
                .Index(t => t.CategoryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Blog.Votes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Plus = c.Boolean(nullable: false),
                        PostId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Blog.Posts", t => t.PostId)
                .ForeignKey("Membership.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Site.Configs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 50),
                        Value = c.String(nullable: false, maxLength: 100),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Blog.TagPosts",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        Post_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Post_Id })
                .ForeignKey("Blog.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("Blog.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Post_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Blog.Posts", "Category_Id", "Blog.Categories");
            DropForeignKey("Blog.Votes", "UserId", "Membership.Users");
            DropForeignKey("Blog.Votes", "PostId", "Blog.Posts");
            DropForeignKey("Blog.Posts", "UserId", "Membership.Users");
            DropForeignKey("Blog.Posts", "TopicId", "Blog.Topics");
            DropForeignKey("Blog.Topics", "UserId", "Membership.Users");
            DropForeignKey("Blog.Topics", "CategoryId", "Blog.Categories");
            DropForeignKey("Blog.TagPosts", "Post_Id", "Blog.Posts");
            DropForeignKey("Blog.TagPosts", "Tag_Id", "Blog.Tags");
            DropForeignKey("Blog.Posts", "HeaderImageId", "Blog.Files");
            DropForeignKey("Blog.Comments", "PostId", "Blog.Posts");
            DropForeignKey("Blog.Comments", "ParentId", "Blog.Comments");
            DropForeignKey("Blog.Files", "Post_Id", "Blog.Posts");
            DropForeignKey("Blog.Categories", "ParentId", "Blog.Categories");
            DropForeignKey("Blog.Categories", "AvatarId", "Blog.Files");
            DropForeignKey("Blog.Files", "UserId", "Membership.Users");
            DropForeignKey("Membership.Profiles", "UserId", "Membership.Users");
            DropForeignKey("Membership.Profiles", "AvatarId", "Blog.Files");
            DropIndex("Blog.TagPosts", new[] { "Post_Id" });
            DropIndex("Blog.TagPosts", new[] { "Tag_Id" });
            DropIndex("Blog.Votes", new[] { "UserId" });
            DropIndex("Blog.Votes", new[] { "PostId" });
            DropIndex("Blog.Topics", new[] { "UserId" });
            DropIndex("Blog.Topics", new[] { "CategoryId" });
            DropIndex("Blog.Comments", new[] { "ParentId" });
            DropIndex("Blog.Comments", new[] { "PostId" });
            DropIndex("Blog.Posts", new[] { "Category_Id" });
            DropIndex("Blog.Posts", new[] { "HeaderImageId" });
            DropIndex("Blog.Posts", new[] { "UserId" });
            DropIndex("Blog.Posts", new[] { "TopicId" });
            DropIndex("Membership.Profiles", new[] { "AvatarId" });
            DropIndex("Membership.Profiles", new[] { "UserId" });
            DropIndex("Blog.Files", new[] { "Post_Id" });
            DropIndex("Blog.Files", new[] { "UserId" });
            DropIndex("Blog.Categories", new[] { "AvatarId" });
            DropIndex("Blog.Categories", new[] { "ParentId" });
            DropTable("Blog.TagPosts");
            DropTable("Site.Configs");
            DropTable("Blog.Votes");
            DropTable("Blog.Topics");
            DropTable("Blog.Tags");
            DropTable("Blog.Comments");
            DropTable("Blog.Posts");
            DropTable("Membership.Profiles");
            DropTable("Membership.Users");
            DropTable("Blog.Files");
            DropTable("Blog.Categories");
        }
    }
}
