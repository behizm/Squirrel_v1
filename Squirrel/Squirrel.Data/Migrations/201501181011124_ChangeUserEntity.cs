namespace Squirrel.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("Membership.Users", "ProfileId");
        }
        
        public override void Down()
        {
            AddColumn("Membership.Users", "ProfileId", c => c.Guid());
        }
    }
}
