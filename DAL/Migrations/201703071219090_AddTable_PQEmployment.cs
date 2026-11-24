namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEmployment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEmployments",
                c => new
                    {
                        EmploymentRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        EV_CandidateName = c.String(nullable: false, maxLength: 100),
                        EV_SecuritasReferenceID = c.String(maxLength: 50),
                        EV_OrganisationName = c.String(maxLength: 100),
                        EV_CmpLocation = c.String(maxLength: 100),
                        EV_CmpNameAddress = c.String(maxLength: 200),
                        EV_EmployeeName = c.String(maxLength: 100),
                        EV_EmployeeCode = c.String(maxLength: 50),
                        EV_EmploymentPeriod = c.String(maxLength: 100),
                        EV_CandidateTenureWorked = c.String(maxLength: 100),
                        EV_EmployeeTenure = c.String(maxLength: 100),
                        EV_Designation = c.String(maxLength: 50),
                        EV_EmpType = c.String(maxLength: 50),
                        EV_Department = c.String(maxLength: 100),
                        EV_CmpCost = c.String(maxLength: 50),
                        EV_last_sal = c.String(maxLength: 50),
                        EV_leavingReason = c.String(maxLength: 100),
                        EV_DiscontinueServiceReason = c.String(maxLength: 100),
                        EV_ReportingManagerName = c.String(maxLength: 100),
                        EV_Rptng_Mgr_Name_Ph = c.String(maxLength: 100),
                        EV_Rptng_Mgr_Name_ConDet = c.String(maxLength: 100),
                        EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = c.String(maxLength: 200),
                        EV_Supervisor = c.String(maxLength: 100),
                        EV_Sup_Name_Dsg = c.String(maxLength: 150),
                        EV_FormalitiesExit = c.String(maxLength: 100),
                        EV_FullFinalFormalities = c.String(maxLength: 100),
                        EV_ExitFormalitiesPending = c.String(maxLength: 100),
                        EV_HandledResponsibilities = c.String(maxLength: 200),
                        EV_ResponsibilitiesHandled = c.String(maxLength: 200),
                        EV_ExprLetter = c.Byte(nullable: false),
                        EV_RelievLetter = c.Byte(nullable: false),
                        EV_OfferLetter = c.Byte(nullable: false),
                        EV_AppointLetter = c.Byte(nullable: false),
                        EV_OtherProof = c.Byte(nullable: false),
                        EV_OtherDetails = c.String(maxLength: 200),
                        EV_OtherDetails2 = c.String(maxLength: 200),
                        EV_OtherDetails3 = c.String(maxLength: 200),
                        EV_OtherDetails4 = c.String(maxLength: 200),
                        EV_OtherDetails5 = c.String(maxLength: 200),
                        EV_OtherDetails6 = c.String(maxLength: 200),
                        EV_OtherDetails7 = c.String(maxLength: 200),
                        EV_OtherDetails8 = c.String(maxLength: 200),
                        EV_OtherDetails9 = c.String(maxLength: 200),
                        EV_OtherDetails10 = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.EmploymentRowId)
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
            DropForeignKey("dbo.PQEmployments", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQEmployments", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQEmployments", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQEmployments", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQEmployments", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQEmployments", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQEmployments", new[] { "PersonalRowID" });
            DropIndex("dbo.PQEmployments", new[] { "ClientRowID" });
            DropTable("dbo.PQEmployments");
        }
    }
}
