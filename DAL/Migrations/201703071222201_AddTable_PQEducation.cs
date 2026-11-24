namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEducation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEducations",
                c => new
                    {
                        EducationRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        EV_Cand_Name = c.String(nullable: false, maxLength: 100),
                        EV_Sec_Ref_Id = c.String(maxLength: 50),
                        EV_Cand_Name_Supp_Edu_Doc = c.String(maxLength: 100),
                        EV_Colg_Schl_Inst_Name = c.Int(nullable: false),
                        EV_Colg_Schl_Inst_Add_ConDet = c.String(maxLength: 200),
                        EV_Affl_University_Name = c.Short(nullable: false),
                        EV_Degree_Name = c.Short(nullable: false),
                        EV_Mode_Of_Edu = c.String(maxLength: 50),
                        EV_Enroll_RollNo = c.String(maxLength: 50),
                        EV_RegNo_RegCode = c.String(maxLength: 50),
                        EV_Dur_Course = c.String(maxLength: 50),
                        EV_Course_Status = c.String(maxLength: 50),
                        EV_Specialization = c.String(maxLength: 100),
                        EV_Period_Course = c.String(maxLength: 100),
                        EV_Yr_Passing = c.String(maxLength: 10),
                        EV_Division_CGPA = c.String(maxLength: 50),
                        EV_DegreeCertfict = c.Byte(nullable: false),
                        EV_ProvisionalCertfict = c.Byte(nullable: false),
                        EV_GradeSheet = c.Byte(nullable: false),
                        EV_AllMarksCards = c.Byte(nullable: false),
                        EV_DocStatColgNameNo = c.Byte(nullable: false),
                        EV_OtherProof = c.Byte(nullable: false),
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
                        ATA_CID_No = c.String(maxLength: 50),
                        ATA_Cmpny_Addr = c.String(maxLength: 200),
                        CreatedBy = c.Short(nullable: false),
                        CreatedDate = c.DateTime(),
                        OutDate = c.DateTime(),
                        InternalOutDate = c.DateTime(),
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
                        CaseStatus = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                        MgrAllocatedBy = c.Short(nullable: false),
                        MgrAllocatedTo = c.Short(nullable: false),
                        MgrAllocatedDate = c.DateTime(),
                        TLAllocatedBy = c.Short(nullable: false),
                        TLAllocatedTo = c.Short(nullable: false),
                        TLAllocatedDate = c.DateTime(),
                        TMAllocatedBy = c.Short(nullable: false),
                        TMAllocatedTo = c.Short(nullable: false),
                        TMAllocatedDate = c.DateTime(),
                        TMQCAllocatedBy = c.Short(nullable: false),
                        TMQCAllocatedTo = c.Short(nullable: false),
                        TMQCAllocatedDate = c.DateTime(),
                        PVMgrAllocatedBy = c.Short(nullable: false),
                        PVMgrAllocatedTo = c.Short(nullable: false),
                        PVMgrAllocatedDate = c.DateTime(),
                        PVTLAllocatedBy = c.Short(nullable: false),
                        PVTLAllocatedTo = c.Short(nullable: false),
                        PVTLAllocatedDate = c.DateTime(),
                        PVTMAllocatedBy = c.Short(nullable: false),
                        PVTMAllocatedTo = c.Short(nullable: false),
                        PVTMAllocatedDate = c.DateTime(),
                        PVTMQCAllocatedBy = c.Short(nullable: false),
                        PVTMQCAllocatedTo = c.Short(nullable: false),
                        PVTMQCAllocatedDate = c.DateTime(),
                        PVFVAllocatedBy = c.Short(nullable: false),
                        PVFVQAllocatedTo = c.Short(nullable: false),
                        PVFVAllocatedDate = c.DateTime(),
                        Verify = c.Byte(nullable: false),
                        VerifiedBy = c.Short(nullable: false),
                        VerifiedDate = c.DateTime(),
                        InfSuffRaiseBy = c.Short(nullable: false),
                        InfSuffRaisedDate = c.DateTime(),
                        InfSuffClearBy = c.Short(nullable: false),
                        InfSuffClearDate = c.DateTime(),
                        Mailto = c.String(maxLength: 200),
                        MailtoClient = c.String(maxLength: 200),
                        MailedBy = c.String(maxLength: 100),
                        ClientComment = c.String(maxLength: 100),
                        ClienINFClearDate = c.DateTime(),
                        INFRemarks = c.String(maxLength: 200),
                        StoppedCheckStatus = c.Byte(nullable: false),
                        StoppedCheckBy = c.Short(nullable: false),
                        StoppedCheckDate = c.DateTime(),
                        StoppedCheckClearBy = c.Short(nullable: false),
                        StoppedCheckClearDate = c.DateTime(),
                        ReportWritterBy = c.Short(nullable: false),
                        ReportWritterDate = c.DateTime(),
                        RWQCManageBy = c.Short(nullable: false),
                        RWQCManageDate = c.DateTime(),
                        RWQCTeamLeaderBy = c.Short(nullable: false),
                        RWQCTeamLeaderDate = c.DateTime(),
                        RWQCTeamMemberBy = c.Short(nullable: false),
                        RWQCTeamMemberDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EducationRowId)
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
            DropForeignKey("dbo.PQEducations", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQEducations", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQEducations", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQEducations", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQEducations", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQEducations", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQEducations", new[] { "PersonalRowID" });
            DropIndex("dbo.PQEducations", new[] { "ClientRowID" });
            DropTable("dbo.PQEducations");
        }
    }
}
