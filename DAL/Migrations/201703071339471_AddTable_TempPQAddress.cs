namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQAddresses",
                c => new
                    {
                        AddressRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        AV_Cand_Name = c.String(maxLength: 100),
                        AV_Sec_Ref_No = c.String(maxLength: 50),
                        AV_Add = c.String(maxLength: 200),
                        AV_Add_Line1 = c.String(maxLength: 200),
                        AV_Add_Line2 = c.String(maxLength: 200),
                        AV_Buldng_Street_name = c.String(maxLength: 100),
                        AV_State = c.Short(nullable: false),
                        AV_District = c.Short(nullable: false),
                        AV_City = c.Int(nullable: false),
                        AV_Pincode = c.Int(nullable: false),
                        AV_Landmark = c.String(maxLength: 100),
                        AV_Add_Type = c.String(maxLength: 100),
                        AV_Dur_of_Stay = c.String(maxLength: 100),
                        AV_Owner_Name = c.String(maxLength: 100),
                        AV_Owner_ConNo = c.String(maxLength: 20),
                        AV_NickName = c.String(maxLength: 50),
                        AV_Name_Chngd_Effect_Frm = c.String(maxLength: 50),
                        AV_Doc_Prov_Proof_Add = c.String(maxLength: 200),
                        AddressProofFileName = c.String(maxLength: 200),
                        AV_Doc_Details = c.String(maxLength: 200),
                        DocumentDetailsFileName = c.String(maxLength: 200),
                        AV_RationCard = c.Byte(nullable: false),
                        AV_TelephoneBill = c.Byte(nullable: false),
                        AV_RentAgreement = c.Byte(nullable: false),
                        AV_Passport = c.Byte(nullable: false),
                        AV_ElectricityBill = c.Byte(nullable: false),
                        AV_GasSupplyBill = c.Byte(nullable: false),
                        AV_WaterBill = c.Byte(nullable: false),
                        AV_OtherProof = c.Byte(nullable: false),
                        AV_OtherDetails = c.String(maxLength: 200),
                        AV_OtherDetails2 = c.String(maxLength: 200),
                        AV_OtherDetails3 = c.String(maxLength: 200),
                        AV_OtherDetails4 = c.String(maxLength: 200),
                        AV_OtherDetails5 = c.String(maxLength: 200),
                        AV_OtherDetails6 = c.String(maxLength: 200),
                        AV_OtherDetails7 = c.String(maxLength: 200),
                        AV_OtherDetails8 = c.String(maxLength: 200),
                        AV_OtherDetails9 = c.String(maxLength: 200),
                        AV_OtherDetails10 = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.AddressRowID)
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
            DropForeignKey("dbo.TempPQAddresses", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQAddresses", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQAddresses", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQAddresses", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQAddresses", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQAddresses", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQAddresses", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQAddresses", new[] { "ClientRowID" });
            DropTable("dbo.TempPQAddresses");
        }
    }
}
