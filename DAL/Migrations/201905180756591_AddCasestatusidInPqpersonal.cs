namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCasestatusidInPqpersonal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQPersonals", "CaseStausID", c => c.Int(nullable: false));
            DropColumn("dbo.PQPersonals", "CaseStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQPersonals", "CaseStatusId", c => c.Int(nullable: false));
            DropColumn("dbo.PQPersonals", "CaseStausID");
        }
    }
}
