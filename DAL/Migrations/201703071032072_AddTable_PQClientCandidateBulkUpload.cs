namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientCandidateBulkUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientCandidateBulkUploads",
                c => new
                    {
                        CBulkUploadRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        ClientCheckRowID = c.Int(nullable: false),
                        ExcelFileName = c.String(maxLength: 300),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CBulkUploadRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientCandidateBulkUploads", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQClientCandidateBulkUploads", new[] { "ClientRowID" });
            DropTable("dbo.PQClientCandidateBulkUploads");
        }
    }
}
