namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_PQClientBulkUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQClientBulkUploads", "UploadedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQClientBulkUploads", "UploadedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQClientBulkUploads", "UploadedDate");
            DropColumn("dbo.PQClientBulkUploads", "UploadedBy");
        }
    }
}
