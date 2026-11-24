namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientContract : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientContracts",
                c => new
                    {
                        ClientContractRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        DocumentType = c.String(maxLength: 100),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 300),
                        DocumentUploadFrom = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 200),
                        UploadDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientContractRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientContracts", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQClientContracts", new[] { "ClientRowID" });
            DropTable("dbo.PQClientContracts");
        }
    }
}
