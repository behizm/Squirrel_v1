namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("Membership.Users", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Membership.Users", "IsAdmin");
        }
    }
}
