namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQSpecialCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQSpecialChecks",
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
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
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
            DropForeignKey("dbo.TempPQSpecialChecks", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.TempPQSpecialChecks", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQSpecialChecks", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQSpecialChecks", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQSpecialChecks", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQSpecialChecks", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQSpecialChecks", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQSpecialChecks", new[] { "ClientRowID" });
            DropTable("dbo.TempPQSpecialChecks");
        }
    }
}
