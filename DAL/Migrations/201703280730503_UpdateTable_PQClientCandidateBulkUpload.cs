namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_PQClientCandidateBulkUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQClientCandidateBulkUploads", "ImportBy", c => c.String(maxLength: 30));
            AddColumn("dbo.PQClientCandidateBulkUploads", "Remarks", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQClientCandidateBulkUploads", "Remarks");
            DropColumn("dbo.PQClientCandidateBulkUploads", "ImportBy");
        }
    }
}
