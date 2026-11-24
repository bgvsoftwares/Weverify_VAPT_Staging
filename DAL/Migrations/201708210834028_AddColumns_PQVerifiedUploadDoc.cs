namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns_PQVerifiedUploadDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQVerifiedUploadDocs", "InsuffDoc", c => c.Byte(nullable: false));
            AddColumn("dbo.PQVerifiedUploadDocs", "RptArtiFacts", c => c.Byte(nullable: false));
            AddColumn("dbo.PQVerifiedUploadDocs", "RptArtiFactsDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQVerifiedUploadDocs", "RptArtiFactsDate");
            DropColumn("dbo.PQVerifiedUploadDocs", "RptArtiFacts");
            DropColumn("dbo.PQVerifiedUploadDocs", "InsuffDoc");
        }
    }
}
