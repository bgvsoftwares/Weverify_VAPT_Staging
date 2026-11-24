namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnConvertIntoForeignkey_TeamMember : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TeamMembers", "BORowID");
            AddForeignKey("dbo.TeamMembers", "BORowID", "dbo.MasterCompanyBranches", "BORowID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMembers", "BORowID", "dbo.MasterCompanyBranches");
            DropIndex("dbo.TeamMembers", new[] { "BORowID" });
        }
    }
}
