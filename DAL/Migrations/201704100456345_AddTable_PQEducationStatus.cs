namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEducationStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEducationStatus",
                c => new
                    {
                        EducationStatusRowID = c.Int(nullable: false, identity: true),
                        EducationRowId = c.Int(nullable: false),
                        DEMgrAllocatedBy = c.Short(nullable: false),
                        DETLAllocatedTo = c.Short(nullable: false),
                        DETMCheckStatus = c.String(maxLength: 20),
                        DETMAllocatedTo = c.Short(nullable: false),
                        DETMAllocatedDate = c.DateTime(),
                        DETMQCCheckStatus = c.String(maxLength: 20),
                        DETMQCAllocatedTo = c.Short(nullable: false),
                        DETMQCAllocatedDate = c.DateTime(),
                        MgrCheckStatus = c.String(maxLength: 20),
                        MgrAllocatedTo = c.Short(nullable: false),
                        MgrAllocatedDate = c.DateTime(),
                        TLCheckStatus = c.String(maxLength: 20),
                        TLAllocatedTo = c.Short(nullable: false),
                        TLAllocatedDate = c.DateTime(),
                        TMCheckStatus = c.String(maxLength: 20),
                        TMAllocatedTo = c.Short(nullable: false),
                        TMAllocatedDate = c.DateTime(),
                        TMQCCheckStatus = c.String(maxLength: 20),
                        TMQCAllocatedTo = c.Short(nullable: false),
                        TMQCAllocatedDate = c.DateTime(),
                        PVMgrCheckStatus = c.String(maxLength: 20),
                        PVMgrAllocatedTo = c.Short(nullable: false),
                        PVMgrAllocatedDate = c.DateTime(),
                        PVTLCheckStatus = c.String(maxLength: 20),
                        PVTLAllocatedTo = c.Short(nullable: false),
                        PVTLAllocatedDate = c.DateTime(),
                        PVTMCheckStatus = c.String(maxLength: 20),
                        PVTMAllocatedTo = c.Short(nullable: false),
                        PVTMAllocatedDate = c.DateTime(),
                        PVTMQCCheckStatus = c.String(maxLength: 20),
                        PVTMQCAllocatedTo = c.Short(nullable: false),
                        PVTMQCAllocatedDate = c.DateTime(),
                        PTRMgrCheckStatus = c.String(maxLength: 20),
                        PTRMgrAllocatedTo = c.Short(nullable: false),
                        PTRMgrAllocatedDate = c.DateTime(),
                        PTRTLCheckStatus = c.String(maxLength: 20),
                        PTRTLAllocatedTo = c.Short(nullable: false),
                        PTRTLAllocatedDate = c.DateTime(),
                        PTRTMCheckStatus = c.String(maxLength: 20),
                        PTRTMAllocatedTo = c.Short(nullable: false),
                        PTRTMAllocatedDate = c.DateTime(),
                        InfSuffRaiseRemarks = c.String(maxLength: 200),
                        InfSuffRaiseBy = c.Short(nullable: false),
                        InfSuffRaisedDate = c.DateTime(),
                        InfSuffClearComments = c.String(maxLength: 200),
                        InfSuffClearBy = c.Short(nullable: false),
                        InfSuffClearDate = c.DateTime(),
                        ClientInfSuffClearComments = c.String(maxLength: 200),
                        ClienInfSuffClearDate = c.DateTime(),
                        StoppedCheckRemarks = c.String(maxLength: 200),
                        StoppedCheckBy = c.Short(nullable: false),
                        StoppedCheckDate = c.DateTime(),
                        StoppedCheckClearComments = c.String(maxLength: 200),
                        StoppedCheckClearBy = c.Short(nullable: false),
                        StoppedCheckClearDate = c.DateTime(),
                        RWCheckStatus = c.String(maxLength: 20),
                        ReportWritterBy = c.Short(nullable: false),
                        ReportWritterDate = c.DateTime(),
                        RWQCMgrCheckStatus = c.String(maxLength: 20),
                        RWQCManagerBy = c.Short(nullable: false),
                        RWQCManagerDate = c.DateTime(),
                        RWQCTLCheckStatus = c.String(maxLength: 20),
                        RWQCTeamLeaderBy = c.Short(nullable: false),
                        RWQCTeamLeaderDate = c.DateTime(),
                        RWQCTMCheckStatus = c.String(maxLength: 20),
                        RWQCTeamMemberBy = c.Short(nullable: false),
                        RWQCTeamMemberDate = c.DateTime(),
                        ReportSendStatus = c.String(maxLength: 20),
                        ReportSendDate = c.DateTime(),
                        OTherCheckStatus = c.String(maxLength: 20),
                        OTherMemberBy = c.Short(nullable: false),
                        OTherMemberDate = c.DateTime(),
                        OTher1CheckStatus = c.String(maxLength: 20),
                        OTher1MemberBy = c.Short(nullable: false),
                        OTher1MemberDate = c.DateTime(),
                        Remarks = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.EducationStatusRowID)
                .ForeignKey("dbo.PQEducations", t => t.EducationRowId)
                .Index(t => t.EducationRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEducationStatus", "EducationRowId", "dbo.PQEducations");
            DropIndex("dbo.PQEducationStatus", new[] { "EducationRowId" });
            DropTable("dbo.PQEducationStatus");
        }
    }
}
