namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQAddressVer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQAddressVers",
                c => new
                    {
                        AddressVerRowID = c.Int(nullable: false, identity: true),
                        AddressRowID = c.Int(nullable: false),
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
                        AV_2_3BHK = c.String(maxLength: 50),
                        AV_Applcnt_Known_Hw_Mny_Yrs = c.String(maxLength: 50),
                        AV_Resp_Name = c.String(maxLength: 100),
                        AV_Resp_Relation_With_Cand = c.String(maxLength: 50),
                        AV_Resp_ConDet = c.String(maxLength: 200),
                        AV_Resp_ConNo = c.String(maxLength: 20),
                        AV_Neighbour_Name_No1 = c.String(maxLength: 100),
                        AV_Neighbour_Name_No2 = c.String(maxLength: 100),
                        AV_Resp_Remarks = c.String(maxLength: 200),
                        AV_Security_Confirm = c.String(maxLength: 100),
                        AV_Owned_Rent_Hypoth = c.String(maxLength: 100),
                        AV_New_Add_Det_Obtain = c.String(maxLength: 200),
                        AV_House_Photo_taken = c.String(maxLength: 20),
                        HousePhotographName = c.String(maxLength: 100),
                        AV_Buldng_Status = c.String(maxLength: 100),
                        AV_Family_Member = c.Byte(nullable: false),
                        AV_Color_of_Buldng = c.String(maxLength: 30),
                        AV_Interior_Condition = c.String(maxLength: 50),
                        AV_Total_Earning_Member = c.Byte(nullable: false),
                        AV_Ease_Locating = c.String(maxLength: 100),
                        AV_Gate_Color = c.String(maxLength: 30),
                        AV_No_Vech = c.Byte(nullable: false),
                        AV_Applcnt_Demons_Unsual_Behv = c.String(maxLength: 20),
                        AV_Idproof_Prov_Resp = c.String(maxLength: 30),
                        AV_Income_Source = c.String(maxLength: 100),
                        AV_Applcnt_Habitul_Borrow_Neigh = c.String(maxLength: 30),
                        AV_Any_Union_Poltcl_Affl_Fmly = c.String(maxLength: 30),
                        AV_Locality = c.String(maxLength: 50),
                        AV_Moral_Charac_Applcnt = c.String(maxLength: 20),
                        AV_Sec_field_Exe_Name = c.String(maxLength: 100),
                        AV_Date_Verification = c.DateTime(),
                        AV_Time_Verification = c.String(maxLength: 20),
                        AV_Any_Other_Cmmnts_AV = c.String(maxLength: 200),
                        AV_OtherDetails6 = c.String(maxLength: 200),
                        AV_OtherDetails7 = c.String(maxLength: 200),
                        AV_OtherDetails8 = c.String(maxLength: 200),
                        AV_OtherDetails9 = c.String(maxLength: 200),
                        AV_OtherDetails10 = c.String(maxLength: 200),
                        AV_OtherDetails11 = c.String(maxLength: 200),
                        AV_OtherDetails12 = c.String(maxLength: 200),
                        AV_OtherDetails13 = c.String(maxLength: 200),
                        AV_OtherDetails14 = c.String(maxLength: 200),
                        AV_OtherDetails15 = c.String(maxLength: 200),
                        ATA_CID_No = c.String(maxLength: 50),
                        ATA_Cmpny_Addr = c.String(maxLength: 200),
                        ATA_ApprxEmpWorking = c.String(maxLength: 100),
                        ATA_Cmpny_Website = c.String(maxLength: 200),
                        ATA_Decoy_Call = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.AddressVerRowID)
                .ForeignKey("dbo.PQAddresses", t => t.AddressRowID)
                .Index(t => t.AddressRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQAddressVers", "AddressRowID", "dbo.PQAddresses");
            DropIndex("dbo.PQAddressVers", new[] { "AddressRowID" });
            DropTable("dbo.PQAddressVers");
        }
    }
}
