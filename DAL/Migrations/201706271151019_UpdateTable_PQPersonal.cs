namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_PQPersonal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQPersonals", "DEScanCaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "DECaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "DEQCCaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "RWQCMgrCaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "RWQCManagerTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQPersonals", "RWQCManagerDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "RWQCTLCaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "RWQCTLTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQPersonals", "RWQCTLDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "RWQCTMCaseStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "RWQCTMTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQPersonals", "RWQCTMDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "FinalRptSendStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "FinalRptSendDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQPersonals", "FinalRptSendDate");
            DropColumn("dbo.PQPersonals", "FinalRptSendStatus");
            DropColumn("dbo.PQPersonals", "RWQCTMDate");
            DropColumn("dbo.PQPersonals", "RWQCTMTo");
            DropColumn("dbo.PQPersonals", "RWQCTMCaseStatus");
            DropColumn("dbo.PQPersonals", "RWQCTLDate");
            DropColumn("dbo.PQPersonals", "RWQCTLTo");
            DropColumn("dbo.PQPersonals", "RWQCTLCaseStatus");
            DropColumn("dbo.PQPersonals", "RWQCManagerDate");
            DropColumn("dbo.PQPersonals", "RWQCManagerTo");
            DropColumn("dbo.PQPersonals", "RWQCMgrCaseStatus");
            DropColumn("dbo.PQPersonals", "DEQCCaseStatus");
            DropColumn("dbo.PQPersonals", "DECaseStatus");
            DropColumn("dbo.PQPersonals", "DEScanCaseStatus");
        }
    }
}
