namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEducationVer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEducationVers",
                c => new
                    {
                        EducationVerRowID = c.Int(nullable: false, identity: true),
                        EducationRowID = c.Int(nullable: false),
                        EV_Cand_Name = c.String(maxLength: 100),
                        EV_Cand_Name_Supp_Edu_Doc = c.String(maxLength: 100),
                        EV_Colg_Schl_Inst_Name = c.String(maxLength: 100),
                        EV_Colg_Schl_Inst_Add_ConDet = c.String(maxLength: 100),
                        EV_Affl_University_Name = c.String(maxLength: 100),
                        EV_Degree_Name = c.String(maxLength: 100),
                        EV_Mode_Of_Edu = c.String(maxLength: 100),
                        EV_Enroll_RollNo = c.String(maxLength: 100),
                        EV_RegNo_RegCode = c.String(maxLength: 100),
                        EV_Dur_Course = c.String(maxLength: 100),
                        EV_Course_Status = c.String(maxLength: 100),
                        EV_Specialization = c.String(maxLength: 200),
                        EV_Period_Course = c.String(maxLength: 100),
                        EV_Yr_Passing = c.String(maxLength: 100),
                        EV_Rslt_Obtain = c.String(maxLength: 100),
                        EV_Mode_of_Verf = c.String(maxLength: 100),
                        EV_Refre_Name_Dsg = c.String(maxLength: 100),
                        EV_Refre_Eid = c.String(maxLength: 100),
                        EV_Refre_ConNo = c.String(maxLength: 100),
                        EV_Attch_Doc_Genuine = c.String(maxLength: 100),
                        EV_Any_Other_Cmmnts_EduV = c.String(maxLength: 100),
                        EV_Date_Time_Verf = c.DateTime(),
                        EV_Verb_Verf_Date = c.DateTime(),
                        EV_Wrtn_Verf_Date = c.DateTime(),
                        EV_Other_Remarks = c.String(maxLength: 100),
                        EV_Division_CGPA = c.String(maxLength: 100),
                        EV_Official_Fee_Proof = c.String(maxLength: 100),
                        EV_Colg_Unvrsty_Affl = c.String(maxLength: 100),
                        EV_Others = c.String(maxLength: 200),
                        EV_Others2 = c.String(maxLength: 200),
                        EV_Others3 = c.String(maxLength: 200),
                        EV_Others4 = c.String(maxLength: 200),
                        EV_Others5 = c.String(maxLength: 200),
                        EV_Others6 = c.String(maxLength: 200),
                        EV_Others7 = c.String(maxLength: 200),
                        EV_Others8 = c.String(maxLength: 200),
                        EV_Others9 = c.String(maxLength: 200),
                        EV_Others10 = c.String(maxLength: 200),
                        EV_Others11 = c.String(maxLength: 200),
                        EV_Others12 = c.String(maxLength: 200),
                        EV_Others13 = c.String(maxLength: 200),
                        EV_Others14 = c.String(maxLength: 200),
                        EV_Others15 = c.String(maxLength: 200),
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
                        ATA_Cmpny_Website = c.String(maxLength: 100),
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
                .PrimaryKey(t => t.EducationVerRowID)
                .ForeignKey("dbo.PQEducations", t => t.EducationRowID)
                .Index(t => t.EducationRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEducationVers", "EducationRowID", "dbo.PQEducations");
            DropIndex("dbo.PQEducationVers", new[] { "EducationRowID" });
            DropTable("dbo.PQEducationVers");
        }
    }
}
