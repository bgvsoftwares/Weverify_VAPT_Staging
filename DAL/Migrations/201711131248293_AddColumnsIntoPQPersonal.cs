namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsIntoPQPersonal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQPersonals", "IsReportUploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.PQPersonals", "ReportUploadedName", c => c.String(maxLength: 100));
            AddColumn("dbo.PQPersonals", "ReportUploadedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQPersonals", "ReportUploadedDate");
            DropColumn("dbo.PQPersonals", "ReportUploadedName");
            DropColumn("dbo.PQPersonals", "IsReportUploaded");
        }
    }
}
