namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_PQVerifiedUploadDoc_PQVerifiedUploadCaseDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQVerifiedUploadCaseDocs", "PdfConvertedToImage", c => c.Boolean(nullable: false));
            AddColumn("dbo.PQVerifiedUploadCaseDocs", "ImageFileName", c => c.String(maxLength: 600));
            AddColumn("dbo.PQVerifiedUploadCaseDocs", "RptArtiFacts", c => c.Byte(nullable: false));
            AddColumn("dbo.PQVerifiedUploadCaseDocs", "RptArtiFactsDate", c => c.DateTime());
            AddColumn("dbo.PQVerifiedUploadCaseDocs", "RptDisplayOrder", c => c.Byte(nullable: false));
            AddColumn("dbo.PQVerifiedUploadDocs", "PdfConvertedToImage", c => c.Boolean(nullable: false));
            AddColumn("dbo.PQVerifiedUploadDocs", "ImageFileName", c => c.String(maxLength: 600));
            AddColumn("dbo.PQVerifiedUploadDocs", "RptDisplayOrder", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQVerifiedUploadDocs", "RptDisplayOrder");
            DropColumn("dbo.PQVerifiedUploadDocs", "ImageFileName");
            DropColumn("dbo.PQVerifiedUploadDocs", "PdfConvertedToImage");
            DropColumn("dbo.PQVerifiedUploadCaseDocs", "RptDisplayOrder");
            DropColumn("dbo.PQVerifiedUploadCaseDocs", "RptArtiFactsDate");
            DropColumn("dbo.PQVerifiedUploadCaseDocs", "RptArtiFacts");
            DropColumn("dbo.PQVerifiedUploadCaseDocs", "ImageFileName");
            DropColumn("dbo.PQVerifiedUploadCaseDocs", "PdfConvertedToImage");
        }
    }
}
