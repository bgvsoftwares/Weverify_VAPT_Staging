namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns_PQPersonal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQPersonals", "InterimRptSendStatus", c => c.String(maxLength: 20));
            AddColumn("dbo.PQPersonals", "InterimRptSendDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "DEScanCompletedDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "DECompletedDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "DEQCCompletedDate", c => c.DateTime());
            AddColumn("dbo.PQPersonals", "RWQCTMCompletedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQPersonals", "RWQCTMCompletedDate");
            DropColumn("dbo.PQPersonals", "DEQCCompletedDate");
            DropColumn("dbo.PQPersonals", "DECompletedDate");
            DropColumn("dbo.PQPersonals", "DEScanCompletedDate");
            DropColumn("dbo.PQPersonals", "InterimRptSendDate");
            DropColumn("dbo.PQPersonals", "InterimRptSendStatus");
        }
    }
}
