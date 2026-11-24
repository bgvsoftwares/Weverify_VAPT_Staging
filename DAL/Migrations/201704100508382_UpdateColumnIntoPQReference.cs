namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQReference : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQReferences", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQReferences", "CaseStatus");
            DropColumn("dbo.PQReferences", "MgrAllocatedBy");
            DropColumn("dbo.PQReferences", "MgrAllocatedTo");
            DropColumn("dbo.PQReferences", "MgrAllocatedDate");
            DropColumn("dbo.PQReferences", "TLAllocatedBy");
            DropColumn("dbo.PQReferences", "TLAllocatedTo");
            DropColumn("dbo.PQReferences", "TLAllocatedDate");
            DropColumn("dbo.PQReferences", "TMAllocatedBy");
            DropColumn("dbo.PQReferences", "TMAllocatedTo");
            DropColumn("dbo.PQReferences", "TMAllocatedDate");
            DropColumn("dbo.PQReferences", "TMQCAllocatedBy");
            DropColumn("dbo.PQReferences", "TMQCAllocatedTo");
            DropColumn("dbo.PQReferences", "TMQCAllocatedDate");
            DropColumn("dbo.PQReferences", "PVMgrAllocatedBy");
            DropColumn("dbo.PQReferences", "PVMgrAllocatedTo");
            DropColumn("dbo.PQReferences", "PVMgrAllocatedDate");
            DropColumn("dbo.PQReferences", "PVTLAllocatedBy");
            DropColumn("dbo.PQReferences", "PVTLAllocatedTo");
            DropColumn("dbo.PQReferences", "PVTLAllocatedDate");
            DropColumn("dbo.PQReferences", "PVTMAllocatedBy");
            DropColumn("dbo.PQReferences", "PVTMAllocatedTo");
            DropColumn("dbo.PQReferences", "PVTMAllocatedDate");
            DropColumn("dbo.PQReferences", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQReferences", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQReferences", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQReferences", "PVFVAllocatedBy");
            DropColumn("dbo.PQReferences", "PVFVQAllocatedTo");
            DropColumn("dbo.PQReferences", "PVFVAllocatedDate");
            DropColumn("dbo.PQReferences", "Verify");
            DropColumn("dbo.PQReferences", "VerifiedBy");
            DropColumn("dbo.PQReferences", "VerifiedDate");
            DropColumn("dbo.PQReferences", "InfSuffRaiseBy");
            DropColumn("dbo.PQReferences", "InfSuffRaisedDate");
            DropColumn("dbo.PQReferences", "InfSuffClearBy");
            DropColumn("dbo.PQReferences", "InfSuffClearDate");
            DropColumn("dbo.PQReferences", "Mailto");
            DropColumn("dbo.PQReferences", "MailtoClient");
            DropColumn("dbo.PQReferences", "MailedBy");
            DropColumn("dbo.PQReferences", "ClientComment");
            DropColumn("dbo.PQReferences", "ClienINFClearDate");
            DropColumn("dbo.PQReferences", "INFRemarks");
            DropColumn("dbo.PQReferences", "StoppedCheckStatus");
            DropColumn("dbo.PQReferences", "StoppedCheckBy");
            DropColumn("dbo.PQReferences", "StoppedCheckDate");
            DropColumn("dbo.PQReferences", "StoppedCheckClearBy");
            DropColumn("dbo.PQReferences", "StoppedCheckClearDate");
            DropColumn("dbo.PQReferences", "ReportWritterBy");
            DropColumn("dbo.PQReferences", "ReportWritterDate");
            DropColumn("dbo.PQReferences", "RWQCManageBy");
            DropColumn("dbo.PQReferences", "RWQCManageDate");
            DropColumn("dbo.PQReferences", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQReferences", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQReferences", "RWQCTeamMemberBy");
            DropColumn("dbo.PQReferences", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQReferences", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQReferences", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQReferences", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQReferences", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQReferences", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQReferences", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQReferences", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQReferences", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQReferences", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQReferences", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQReferences", "ReWorkCheckStatus");
        }
    }
}
