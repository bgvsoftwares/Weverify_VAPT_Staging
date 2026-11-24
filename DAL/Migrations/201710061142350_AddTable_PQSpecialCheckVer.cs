namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQSpecialCheckVer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQSpecialCheckVers",
                c => new
                    {
                        SpecialCheckVerRowID = c.Int(nullable: false, identity: true),
                        SpecialCheckRowId = c.Int(nullable: false),
                        SC_Cand_Name = c.String(maxLength: 100),
                        SC_Father_Name = c.String(maxLength: 100),
                        SC_DOB = c.DateTime(),
                        SC_Mode_Verify = c.String(maxLength: 50),
                        SC_Mode_VerOther = c.String(maxLength: 100),
                        SC_Date_Verify = c.DateTime(),
                        SC_AnyOtherComments = c.String(maxLength: 500),
                        SC_Others1 = c.String(maxLength: 200),
                        SC_Others2 = c.String(maxLength: 200),
                        SC_Others3 = c.String(maxLength: 200),
                        SC_Others4 = c.String(maxLength: 200),
                        SC_Others5 = c.String(maxLength: 200),
                        TypeRevert = c.String(maxLength: 50),
                        CheckStatus = c.String(maxLength: 50),
                        Severity = c.String(maxLength: 50),
                        ColorName = c.String(maxLength: 50),
                        ReportComments = c.String(maxLength: 3000),
                        Status = c.Byte(nullable: false),
                        VerifiedBy = c.Short(nullable: false),
                        VerifierDesignation = c.String(maxLength: 100),
                        VerifierContactNo = c.String(maxLength: 20),
                        VerifierMobileNo = c.String(maxLength: 20),
                        VerifierEmailId = c.String(maxLength: 100),
                        VerifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SpecialCheckVerRowID)
                .ForeignKey("dbo.PQSpecialChecks", t => t.SpecialCheckRowId)
                .Index(t => t.SpecialCheckRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQSpecialCheckVers", "SpecialCheckRowId", "dbo.PQSpecialChecks");
            DropIndex("dbo.PQSpecialCheckVers", new[] { "SpecialCheckRowId" });
            DropTable("dbo.PQSpecialCheckVers");
        }
    }
}
