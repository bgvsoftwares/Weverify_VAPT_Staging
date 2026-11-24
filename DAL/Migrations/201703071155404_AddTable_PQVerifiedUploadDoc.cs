namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQVerifiedUploadDoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQVerifiedUploadDocs",
                c => new
                    {
                        VerifiedUploadRowID = c.Long(nullable: false, identity: true),
                        PersonalRowID = c.Int(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        DocumentUploadFrom = c.String(maxLength: 20),
                        DocumentType = c.String(maxLength: 50),
                        FileName = c.String(nullable: false, maxLength: 300),
                        FilePath = c.String(maxLength: 600),
                        Remarks = c.String(maxLength: 500),
                        UploadDate = c.DateTime(),
                        StatusDocxConvert = c.Byte(nullable: false),
                        StatusPDF2DOCXConvert = c.Byte(nullable: false),
                        PDF_FileName = c.String(maxLength: 300),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.VerifiedUploadRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQVerifiedUploadDocs", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQVerifiedUploadDocs", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropIndex("dbo.PQVerifiedUploadDocs", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQVerifiedUploadDocs", new[] { "PersonalRowID" });
            DropTable("dbo.PQVerifiedUploadDocs");
        }
    }
}
