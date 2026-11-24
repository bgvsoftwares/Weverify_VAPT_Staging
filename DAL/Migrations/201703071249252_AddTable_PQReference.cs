namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQReference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQReferences",
                c => new
                    {
                        ReferenceRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        RV_CandidateName = c.String(maxLength: 100),
                        RV_Sec_Ref_Id = c.String(maxLength: 50),
                        RV_OrganisationName = c.String(maxLength: 100),
                        RV_RefereeName = c.String(maxLength: 100),
                        RV_Refere_Desig = c.String(maxLength: 100),
                        RV_Refere_Add = c.String(maxLength: 200),
                        RV_Refre_MobNo = c.String(maxLength: 20),
                        RV_Refre_Eid = c.String(maxLength: 100),
                        RV_Know_Long = c.String(maxLength: 200),
                        RV_Tenure_Emp = c.String(maxLength: 100),
                        RV_Dsg_RC = c.String(maxLength: 100),
                        RV_CTC = c.String(maxLength: 50),
                        RV_Reason_Leav = c.String(maxLength: 200),
                        RV_OtherProof = c.Byte(nullable: false),
                        RV_OtherDetails = c.String(maxLength: 200),
                        RV_OtherDetails2 = c.String(maxLength: 200),
                        RV_OtherDetails3 = c.String(maxLength: 200),
                        RV_OtherDetails4 = c.String(maxLength: 200),
                        RV_OtherDetails5 = c.String(maxLength: 200),
                        RV_OtherDetails6 = c.String(maxLength: 200),
                        RV_OtherDetails7 = c.String(maxLength: 200),
                        RV_OtherDetails8 = c.String(maxLength: 200),
                        RV_OtherDetails9 = c.String(maxLength: 200),
                        RV_OtherDetails10 = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.ReferenceRowID)
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
            DropForeignKey("dbo.PQReferences", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQReferences", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQReferences", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQReferences", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQReferences", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQReferences", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQReferences", new[] { "PersonalRowID" });
            DropIndex("dbo.PQReferences", new[] { "ClientRowID" });
            DropTable("dbo.PQReferences");
        }
    }
}
