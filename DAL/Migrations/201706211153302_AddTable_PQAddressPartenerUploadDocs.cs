namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQAddressPartenerUploadDocs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQAddressPartenerUploadDocs",
                c => new
                    {
                        PartenerUploadRowID = c.Long(nullable: false),
                        PQAddressVarPtrRowId = c.Int(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        DocumentUploadFrom = c.String(maxLength: 200),
                        DocumentType = c.String(maxLength: 50),
                        FileName = c.String(maxLength: 100),
                        FilePath = c.String(maxLength: 300),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.PartenerUploadRowID)
                .ForeignKey("dbo.PQAddressVarParteners", t => t.PQAddressVarPtrRowId)
                .Index(t => t.PQAddressVarPtrRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQAddressPartenerUploadDocs", "PQAddressVarPtrRowId", "dbo.PQAddressVarParteners");
            DropIndex("dbo.PQAddressPartenerUploadDocs", new[] { "PQAddressVarPtrRowId" });
            DropTable("dbo.PQAddressPartenerUploadDocs");
        }
    }
}
