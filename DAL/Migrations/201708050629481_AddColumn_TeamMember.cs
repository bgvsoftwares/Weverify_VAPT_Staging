namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_TeamMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMembers", "BORowID", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamMembers", "BORowID");
        }
    }
}
