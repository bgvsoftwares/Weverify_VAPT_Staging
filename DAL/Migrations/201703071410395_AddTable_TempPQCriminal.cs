namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQCriminal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQCriminals",
                c => new
                    {
                        CriminalRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        CRV_Cand_Name = c.String(nullable: false, maxLength: 100),
                        CRV_Sec_Ref_No = c.String(maxLength: 50),
                        CRV_DOB = c.DateTime(),
                        CRV_Father_Name = c.String(maxLength: 100),
                        CRV_First_Name = c.String(maxLength: 50),
                        CRV_Last_Name = c.String(maxLength: 50),
                        CRV_Nationality = c.String(maxLength: 50),
                        CRV_Name_Chngd_Effect_Frm = c.DateTime(),
                        CRV_Nationality_On_Passport = c.String(maxLength: 50),
                        CRV_Country_of_Issue = c.String(maxLength: 100),
                        CRV_Addr_Type = c.String(maxLength: 50),
                        CRV_Current_Addr = c.String(maxLength: 200),
                        CRV_Prev_Addr1 = c.String(maxLength: 200),
                        CRV_Prev_Addr2 = c.String(maxLength: 200),
                        CRV_Prev_Addr3 = c.String(maxLength: 200),
                        CRV_Prev_Addr4 = c.String(maxLength: 200),
                        CRV_Permanent_Addr = c.String(maxLength: 200),
                        CRV_Period_Of_Stay = c.String(maxLength: 100),
                        CRV_Gender = c.String(maxLength: 20),
                        CRV_Identity_No = c.String(maxLength: 100),
                        CRV_Identity_Prov = c.String(maxLength: 200),
                        CRV_Doc_Received = c.String(maxLength: 200),
                        CRV_IdentityProof = c.Byte(nullable: false),
                        CRV_AddressProof = c.Byte(nullable: false),
                        CRV_AuthrizedLetter = c.Byte(nullable: false),
                        CRV_OtherProof = c.Byte(nullable: false),
                        CRV_Others = c.String(maxLength: 200),
                        CRV_Others2 = c.String(maxLength: 200),
                        CRV_Others3 = c.String(maxLength: 200),
                        CRV_Others4 = c.String(maxLength: 200),
                        CRV_Others5 = c.String(maxLength: 200),
                        CRV_Others6 = c.String(maxLength: 200),
                        CRV_Others7 = c.String(maxLength: 200),
                        CRV_Others8 = c.String(maxLength: 200),
                        CRV_Others9 = c.String(maxLength: 200),
                        CRV_Others10 = c.String(maxLength: 200),
                        ATA_CID_No = c.String(maxLength: 50),
                        ATA_Cmpny_Addr = c.String(maxLength: 200),
                        CreatedBy = c.Short(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CriminalRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.TempPQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPQCriminals", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQCriminals", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQCriminals", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQCriminals", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQCriminals", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQCriminals", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQCriminals", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQCriminals", new[] { "ClientRowID" });
            DropTable("dbo.TempPQCriminals");
        }
    }
}
