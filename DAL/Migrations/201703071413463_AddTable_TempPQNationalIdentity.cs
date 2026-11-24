namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQNationalIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQNationalIdentities",
                c => new
                    {
                        NationalIdentityRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        NIC_Cand_Name = c.String(maxLength: 100),
                        NIC_Sec_Ref_Id = c.String(maxLength: 50),
                        NIC_DOB = c.DateTime(),
                        NIC_DOB1 = c.DateTime(),
                        NIC_ID_No = c.String(maxLength: 50),
                        NIC_Date_Issue = c.DateTime(),
                        NIC_Expiry_Date = c.DateTime(),
                        NIC_Passport_No = c.String(maxLength: 20),
                        NIC_SSN_NO = c.String(maxLength: 50),
                        NIC_Pan_No = c.String(maxLength: 20),
                        NIC_Voter_Id_No = c.String(maxLength: 20),
                        NIC_Dvng_Liscense_No = c.String(maxLength: 20),
                        NIC_Aadhar_Card_No = c.String(maxLength: 20),
                        NIC_Place_Issue = c.String(maxLength: 100),
                        NIC_Place_Residnce = c.String(maxLength: 100),
                        NIC_Type_Id_Card = c.String(maxLength: 100),
                        NIC_Reg_Doc_Type = c.String(maxLength: 100),
                        NIC_RegNo = c.String(maxLength: 100),
                        NIC_PinCode = c.String(maxLength: 10),
                        NIC_Name_Key_Personnel = c.String(maxLength: 100),
                        NIC_AliasFname = c.String(maxLength: 50),
                        NIC_Alias_Lname = c.String(maxLength: 50),
                        NIC_Country_Iss_Passport = c.String(maxLength: 100),
                        NIC_Gender = c.String(maxLength: 20),
                        NIC_Id_CardNo = c.String(maxLength: 50),
                        NIC_Identity_Prov = c.String(maxLength: 100),
                        NIC_Name_Changed_Effect_Frm = c.DateTime(),
                        NIC_Nationality = c.String(maxLength: 100),
                        NIC_AadhaarCard = c.Byte(nullable: false),
                        NIC_DrivingLicense = c.Byte(nullable: false),
                        NIC_PANCard = c.Byte(nullable: false),
                        NIC_Passport = c.Byte(nullable: false),
                        NIC_VoterID = c.Byte(nullable: false),
                        NIC_OtherProof = c.Byte(nullable: false),
                        NIC_OtherDetails = c.String(maxLength: 200),
                        NIC_OtherDetails2 = c.String(maxLength: 200),
                        NIC_OtherDetails3 = c.String(maxLength: 200),
                        NIC_OtherDetails4 = c.String(maxLength: 200),
                        NIC_OtherDetails5 = c.String(maxLength: 200),
                        NIC_OtherDetails6 = c.String(maxLength: 200),
                        NIC_OtherDetails7 = c.String(maxLength: 200),
                        NIC_OtherDetails8 = c.String(maxLength: 200),
                        NIC_OtherDetails9 = c.String(maxLength: 200),
                        NIC_OtherDetails10 = c.String(maxLength: 200),
                        ATA_CID_No = c.String(maxLength: 100),
                        ATA_Cmpny_Addr = c.String(maxLength: 100),
                        CreatedBy = c.Short(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.NationalIdentityRowID)
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
            DropForeignKey("dbo.TempPQNationalIdentities", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQNationalIdentities", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQNationalIdentities", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQNationalIdentities", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQNationalIdentities", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQNationalIdentities", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQNationalIdentities", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQNationalIdentities", new[] { "ClientRowID" });
            DropTable("dbo.TempPQNationalIdentities");
        }
    }
}
