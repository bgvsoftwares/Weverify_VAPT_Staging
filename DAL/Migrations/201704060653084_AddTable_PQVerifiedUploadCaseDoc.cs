namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQVerifiedUploadCaseDoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQVerifiedUploadCaseDocs",
                c => new
                    {
                        CaseDocRowID = c.Long(nullable: false, identity: true),
                        PersonalRowID = c.Int(nullable: false),
                        DocumentUploadFrom = c.String(maxLength: 20),
                        DocumentType = c.String(maxLength: 50),
                        FileName = c.String(nullable: false, maxLength: 300),
                        FilePath = c.String(maxLength: 600),
                        Remarks = c.String(maxLength: 500),
                        UploadDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CaseDocRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.PersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQVerifiedUploadCaseDocs", "PersonalRowID", "dbo.PQPersonals");
            DropIndex("dbo.PQVerifiedUploadCaseDocs", new[] { "PersonalRowID" });
            DropTable("dbo.PQVerifiedUploadCaseDocs");
        }
    }
}
