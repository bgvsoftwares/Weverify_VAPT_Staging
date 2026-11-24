namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQCriminalVer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQCriminalVers",
                c => new
                    {
                        CriminalVerRowId = c.Int(nullable: false, identity: true),
                        CriminalRowID = c.Int(nullable: false),
                        CRV_Cand_Name = c.String(maxLength: 100),
                        CRV_DOB = c.DateTime(nullable: false),
                        CRV_Father_Name = c.String(maxLength: 100),
                        CRV_First_Name = c.String(maxLength: 100),
                        CRV_Last_Name = c.String(maxLength: 100),
                        CRV_Nationality = c.String(maxLength: 100),
                        CRV_Name_Chngd_Effect_Frm = c.String(maxLength: 100),
                        CRV_Nationality_On_Passport = c.String(maxLength: 100),
                        CRV_Country_of_Issue = c.String(maxLength: 100),
                        CRV_Addr_Type = c.String(maxLength: 400),
                        CRV_Current_Addr = c.String(maxLength: 100),
                        CRV_Prev_Addr1 = c.String(maxLength: 200),
                        CRV_Prev_Addr2 = c.String(maxLength: 200),
                        CRV_Prev_Addr3 = c.String(maxLength: 200),
                        CRV_Prev_Addr4 = c.String(maxLength: 200),
                        CRV_Permanent_Addr = c.String(maxLength: 100),
                        CRV_Gender = c.String(maxLength: 30),
                        CRV_Identity_No = c.String(maxLength: 100),
                        CRV_Identity_Prov = c.String(maxLength: 100),
                        CRV_Doc_Received = c.String(maxLength: 100),
                        CRV_Dur_Yr_Covered = c.String(maxLength: 100),
                        CRV_Pol_Station_Name = c.String(maxLength: 100),
                        CRV_Pol_Authority_Name = c.String(maxLength: 100),
                        CRV_Pol_Authority_Desg = c.String(maxLength: 100),
                        CRV_Criminal_Verf = c.String(maxLength: 100),
                        CRV_Type_of_Revert = c.String(maxLength: 100),
                        CRV_Date_Addr_Verif = c.DateTime(nullable: false),
                        CRV_Date_Verf = c.DateTime(nullable: false),
                        CRV_Time_Verf = c.DateTime(nullable: false),
                        CRV_Verfier_Cmmnts = c.String(maxLength: 200),
                        CRV_St_Pol_Most_Wanted = c.String(maxLength: 100),
                        CRV_CBI_Most_Wanted = c.String(maxLength: 100),
                        CRV_Interpol_Most_Wanted = c.String(maxLength: 100),
                        CRV_Allied_Terrorist_Dataset = c.String(maxLength: 100),
                        CRV_Crimes_Dishonest_Dataset = c.String(maxLength: 100),
                        CRV_Global_Corruption_Dataset = c.String(maxLength: 100),
                        CRV_Global_Financ_Rgltry_Dataset = c.String(maxLength: 100),
                        CRV_Global_Fraud_Dataset = c.String(maxLength: 100),
                        CRV_Global_Law_Enforce_Dataset = c.String(maxLength: 100),
                        CRV_Sex_Crime_Dataset = c.String(maxLength: 100),
                        CRV_Violent_Crime_Dataset = c.String(maxLength: 100),
                        CRV_Global_Terrorist_Risk = c.String(maxLength: 100),
                        CRV_Global_Money_Laund = c.String(maxLength: 100),
                        CRV_Civil_Court_Action = c.String(maxLength: 100),
                        CRV_District_Court = c.String(maxLength: 100),
                        CRV_State_Court = c.String(maxLength: 100),
                        CRV_Civil_Court = c.String(maxLength: 100),
                        CRV_Special_Court = c.String(maxLength: 100),
                        CRV_Session_Court = c.String(maxLength: 100),
                        CRV_Tribunals = c.String(maxLength: 100),
                        CRV_Any_Other_Cmmnts_CRV = c.String(maxLength: 200),
                        CRV_Magistrate_Court = c.String(maxLength: 100),
                        CRV_District_Civil_Court = c.String(maxLength: 100),
                        CRV_St_High_Court = c.String(maxLength: 100),
                        CRV_Supreme_Court = c.String(maxLength: 100),
                        CRV_Criminal_Appeal = c.String(maxLength: 100),
                        CRV_Private_Complain_Rpt = c.String(maxLength: 100),
                        CRV_Others6 = c.String(maxLength: 100),
                        CRV_Others7 = c.String(maxLength: 100),
                        CRV_Others8 = c.String(maxLength: 100),
                        CRV_Others9 = c.String(maxLength: 100),
                        CRV_Others10 = c.String(maxLength: 100),
                        CRV_Others11 = c.String(maxLength: 100),
                        CRV_Others12 = c.String(maxLength: 100),
                        CRV_Others13 = c.String(maxLength: 100),
                        CRV_Others14 = c.String(maxLength: 100),
                        CRV_Others15 = c.String(maxLength: 100),
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
                        ATA_NASCOM_Empanelment = c.String(maxLength: 100),
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
                .PrimaryKey(t => t.CriminalVerRowId)
                .ForeignKey("dbo.PQCriminals", t => t.CriminalRowID)
                .Index(t => t.CriminalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQCriminalVers", "CriminalRowID", "dbo.PQCriminals");
            DropIndex("dbo.PQCriminalVers", new[] { "CriminalRowID" });
            DropTable("dbo.PQCriminalVers");
        }
    }
}
