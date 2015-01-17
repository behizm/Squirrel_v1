namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditEntities : DbMigration
    {
        public override void Up()
        {
            AddColumn("Membership.Users", "PasswordHash", c => c.String(nullable: false, maxLength: 100));
            AddColumn("Membership.Users", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Membership.Users", "EditeDate", c => c.DateTime());
            AddColumn("Membership.Users", "LastLogin", c => c.DateTime());
            AddColumn("Membership.Users", "IsLock", c => c.Boolean(nullable: false));
            AddColumn("Membership.Users", "LockDate", c => c.DateTime());
            DropColumn("Membership.Users", "PasswordHashed");
        }
        
        public override void Down()
        {
            AddColumn("Membership.Users", "PasswordHashed", c => c.String(nullable: false, maxLength: 100));
            DropColumn("Membership.Users", "LockDate");
            DropColumn("Membership.Users", "IsLock");
            DropColumn("Membership.Users", "LastLogin");
            DropColumn("Membership.Users", "EditeDate");
            DropColumn("Membership.Users", "IsActive");
            DropColumn("Membership.Users", "PasswordHash");
        }
    }
}
