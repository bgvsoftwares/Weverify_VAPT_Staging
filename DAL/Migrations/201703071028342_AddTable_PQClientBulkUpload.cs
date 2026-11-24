namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientBulkUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientBulkUploads",
                c => new
                    {
                        ClientBulkUploadRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        ExcelFileName = c.String(maxLength: 300),
                        AttachDocName = c.String(maxLength: 300),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientBulkUploadRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientBulkUploads", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQClientBulkUploads", new[] { "ClientRowID" });
            DropTable("dbo.PQClientBulkUploads");
        }
    }
}
