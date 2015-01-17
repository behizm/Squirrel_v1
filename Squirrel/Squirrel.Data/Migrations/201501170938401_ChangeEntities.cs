namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog.Posts", "Abstract", c => c.String(maxLength: 300));
            AddColumn("Blog.Topics", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("Blog.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Blog.Files", "Name", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("Blog.Files", "Address", c => c.String(nullable: false));
            AlterColumn("Blog.Files", "Filename", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Blog.Files", "EditDate", c => c.DateTime());
            AlterColumn("Membership.Users", "Username", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("Membership.Users", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Membership.Users", "PasswordHashed", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Membership.Profiles", "Firstname", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("Membership.Profiles", "Lastname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Membership.Profiles", "EditDate", c => c.DateTime());
            AlterColumn("Blog.Posts", "Body", c => c.String(nullable: false));
            AlterColumn("Blog.Posts", "EditDate", c => c.DateTime());
            AlterColumn("Blog.Comments", "Body", c => c.String(nullable: false));
            AlterColumn("Blog.Tags", "Name", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("Blog.Topics", "EditDate", c => c.DateTime());
            AlterColumn("Site.Configs", "Key", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Site.Configs", "Value", c => c.String(nullable: false, maxLength: 100));
            DropColumn("Blog.Topics", "Body");
        }
        
        public override void Down()
        {
            AddColumn("Blog.Topics", "Body", c => c.String());
            AlterColumn("Site.Configs", "Value", c => c.String());
            AlterColumn("Site.Configs", "Key", c => c.String());
            AlterColumn("Blog.Topics", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("Blog.Tags", "Name", c => c.String());
            AlterColumn("Blog.Comments", "Body", c => c.String());
            AlterColumn("Blog.Posts", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("Blog.Posts", "Body", c => c.String());
            AlterColumn("Membership.Profiles", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("Membership.Profiles", "Lastname", c => c.String());
            AlterColumn("Membership.Profiles", "Firstname", c => c.String());
            AlterColumn("Membership.Users", "PasswordHashed", c => c.String());
            AlterColumn("Membership.Users", "Email", c => c.String());
            AlterColumn("Membership.Users", "Username", c => c.String());
            AlterColumn("Blog.Files", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("Blog.Files", "Filename", c => c.String());
            AlterColumn("Blog.Files", "Address", c => c.String());
            AlterColumn("Blog.Files", "Name", c => c.String());
            AlterColumn("Blog.Categories", "Name", c => c.String());
            DropColumn("Blog.Topics", "Title");
            DropColumn("Blog.Posts", "Abstract");
        }
    }
}
