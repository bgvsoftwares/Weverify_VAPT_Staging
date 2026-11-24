namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQSpecialCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQSpecialChecks",
                c => new
                    {
                        SpecialCheckRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        SC_Cand_Name = c.String(nullable: false, maxLength: 100),
                        SC_Father_Name = c.String(maxLength: 100),
                        SC_SecuritasID = c.String(maxLength: 50),
                        SC_DOB = c.DateTime(),
                        SC_Others1 = c.String(maxLength: 200),
                        SC_Others2 = c.String(maxLength: 200),
                        SC_Others3 = c.String(maxLength: 200),
                        SC_Others4 = c.String(maxLength: 200),
                        SC_Others5 = c.String(maxLength: 200),
                        CreatedBy = c.Short(nullable: false),
                        CreatedDate = c.DateTime(),
                        OutDate = c.DateTime(),
                        InternalOutDate = c.DateTime(),
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
                        ReWorkCheckStatus = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.SpecialCheckRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQSpecialChecks", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQSpecialChecks", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQSpecialChecks", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQSpecialChecks", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQSpecialChecks", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQSpecialChecks", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQSpecialChecks", new[] { "PersonalRowID" });
            DropIndex("dbo.PQSpecialChecks", new[] { "ClientRowID" });
            DropTable("dbo.PQSpecialChecks");
        }
    }
}
