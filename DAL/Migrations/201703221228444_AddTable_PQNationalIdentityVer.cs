namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQNationalIdentityVer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQNationalIdentityVers",
                c => new
                    {
                        NationalIdentityVerRowID = c.Int(nullable: false, identity: true),
                        NationalIdentityRowID = c.Int(nullable: false),
                        NIC_Cand_Name = c.String(maxLength: 100),
                        NIC_Any_Other_Cmmnts_NIC = c.String(maxLength: 200),
                        NIC_DOB1 = c.DateTime(),
                        NIC_ID_No = c.String(maxLength: 100),
                        NIC_Verified_By = c.String(maxLength: 100),
                        NIC_Date_Issue = c.String(maxLength: 100),
                        NIC_Expiry_Date = c.String(maxLength: 100),
                        NIC_Validity_Status = c.String(maxLength: 100),
                        NIC_Passport_No = c.String(maxLength: 100),
                        NIC_SSN_NO = c.String(maxLength: 100),
                        NIC_Pan_No = c.String(maxLength: 100),
                        NIC_Voter_Id_No = c.String(maxLength: 100),
                        NIC_Dvng_Liscense_No = c.String(maxLength: 100),
                        NIC_Aadhar_Card_No = c.String(maxLength: 100),
                        NIC_Place_Issue = c.String(maxLength: 100),
                        NIC_Place_Residnce = c.String(maxLength: 100),
                        NIC_Time_Verf = c.String(maxLength: 100),
                        NIC_Type_Id_Card = c.String(maxLength: 100),
                        NIC_Reg_Doc_Type = c.String(maxLength: 100),
                        NIC_RegNo = c.String(maxLength: 100),
                        NIC_Name_Field_Executive = c.String(maxLength: 100),
                        NIC_PinCode = c.String(maxLength: 100),
                        NIC_Relation_Key_Personal = c.String(maxLength: 100),
                        NIC_Verified_Frm = c.String(maxLength: 100),
                        NIC_Auth_Details_Name = c.String(maxLength: 100),
                        NIC_Dsg = c.String(maxLength: 100),
                        NIC_Eid = c.String(maxLength: 100),
                        NIC_MobNo = c.String(maxLength: 100),
                        NIC_Name_Key_Personnel = c.String(maxLength: 100),
                        NIC_AliasFname = c.String(maxLength: 100),
                        NIC_Alias_Lname = c.String(maxLength: 100),
                        NIC_Country_Iss_Passport = c.String(maxLength: 100),
                        NIC_Date_Verif = c.DateTime(),
                        NIC_Id_CardNo = c.String(maxLength: 100),
                        NIC_Identity_Prov = c.String(maxLength: 100),
                        NIC_Name_Changed_Effect_Frm = c.String(maxLength: 100),
                        NIC_Nationality = c.String(maxLength: 100),
                        NIC_PhotoID_Verif = c.String(maxLength: 100),
                        NIC_Any_Other_Remarks_NIC = c.String(maxLength: 100),
                        NIC_OtherDetails6 = c.String(maxLength: 200),
                        NIC_OtherDetails7 = c.String(maxLength: 200),
                        NIC_OtherDetails8 = c.String(maxLength: 200),
                        NIC_OtherDetails9 = c.String(maxLength: 200),
                        NIC_OtherDetails10 = c.String(maxLength: 200),
                        NIC_OtherDetails11 = c.String(maxLength: 200),
                        NIC_OtherDetails12 = c.String(maxLength: 200),
                        NIC_OtherDetails13 = c.String(maxLength: 200),
                        NIC_OtherDetails14 = c.String(maxLength: 200),
                        NIC_OtherDetails15 = c.String(maxLength: 200),
                        ATA_Reverse_Directory = c.String(maxLength: 200),
                        ATA_Residcl_Commrcl = c.String(maxLength: 200),
                        ATA_Stock_Exchnge = c.String(maxLength: 200),
                        ATA_Telphne_Directry_Srch = c.String(maxLength: 200),
                        ATA_Yellow_Pages = c.String(maxLength: 200),
                        ATA_Who_Domain = c.String(maxLength: 200),
                        ATA_GoogleSearch = c.String(maxLength: 200),
                        ATA_InfrastructureOfCmp = c.String(maxLength: 200),
                        ATA_Just_Dial = c.String(maxLength: 200),
                        ATA_Neighbor_Check1 = c.String(maxLength: 200),
                        ATA_NASCOM_Empanelment = c.String(maxLength: 200),
                        ATA_Photograph = c.String(maxLength: 200),
                        ATA_Physical_Site = c.String(maxLength: 200),
                        ATA_Reg_Company = c.String(maxLength: 200),
                        ATA_PO_Courier_Check = c.String(maxLength: 200),
                        ATA_Network_Sol = c.String(maxLength: 200),
                        ATA_ApprxEmpWorking = c.String(maxLength: 200),
                        ATA_CID_No = c.String(maxLength: 200),
                        ATA_Cmpny_Addr = c.String(maxLength: 200),
                        ATA_Decoy_Call = c.String(maxLength: 200),
                        ATA_Cmpny_Website = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.NationalIdentityVerRowID)
                .ForeignKey("dbo.PQNationalIdentities", t => t.NationalIdentityRowID)
                .Index(t => t.NationalIdentityRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQNationalIdentityVers", "NationalIdentityRowID", "dbo.PQNationalIdentities");
            DropIndex("dbo.PQNationalIdentityVers", new[] { "NationalIdentityRowID" });
            DropTable("dbo.PQNationalIdentityVers");
        }
    }
}
