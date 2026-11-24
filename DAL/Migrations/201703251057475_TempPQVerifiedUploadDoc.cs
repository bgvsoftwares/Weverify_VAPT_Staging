namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempPQVerifiedUploadDoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQVerifiedUploadDocs",
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
                .ForeignKey("dbo.TempPQPersonals", t => t.PersonalRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPQVerifiedUploadDocs", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQVerifiedUploadDocs", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropIndex("dbo.TempPQVerifiedUploadDocs", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQVerifiedUploadDocs", new[] { "PersonalRowID" });
            DropTable("dbo.TempPQVerifiedUploadDocs");
        }
    }
}
