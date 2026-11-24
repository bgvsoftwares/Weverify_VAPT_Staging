namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQEducation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQEducations", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQEducations", "CaseStatus");
            DropColumn("dbo.PQEducations", "MgrAllocatedBy");
            DropColumn("dbo.PQEducations", "MgrAllocatedTo");
            DropColumn("dbo.PQEducations", "MgrAllocatedDate");
            DropColumn("dbo.PQEducations", "TLAllocatedBy");
            DropColumn("dbo.PQEducations", "TLAllocatedTo");
            DropColumn("dbo.PQEducations", "TLAllocatedDate");
            DropColumn("dbo.PQEducations", "TMAllocatedBy");
            DropColumn("dbo.PQEducations", "TMAllocatedTo");
            DropColumn("dbo.PQEducations", "TMAllocatedDate");
            DropColumn("dbo.PQEducations", "TMQCAllocatedBy");
            DropColumn("dbo.PQEducations", "TMQCAllocatedTo");
            DropColumn("dbo.PQEducations", "TMQCAllocatedDate");
            DropColumn("dbo.PQEducations", "PVMgrAllocatedBy");
            DropColumn("dbo.PQEducations", "PVMgrAllocatedTo");
            DropColumn("dbo.PQEducations", "PVMgrAllocatedDate");
            DropColumn("dbo.PQEducations", "PVTLAllocatedBy");
            DropColumn("dbo.PQEducations", "PVTLAllocatedTo");
            DropColumn("dbo.PQEducations", "PVTLAllocatedDate");
            DropColumn("dbo.PQEducations", "PVTMAllocatedBy");
            DropColumn("dbo.PQEducations", "PVTMAllocatedTo");
            DropColumn("dbo.PQEducations", "PVTMAllocatedDate");
            DropColumn("dbo.PQEducations", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQEducations", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQEducations", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQEducations", "PVFVAllocatedBy");
            DropColumn("dbo.PQEducations", "PVFVQAllocatedTo");
            DropColumn("dbo.PQEducations", "PVFVAllocatedDate");
            DropColumn("dbo.PQEducations", "Verify");
            DropColumn("dbo.PQEducations", "VerifiedBy");
            DropColumn("dbo.PQEducations", "VerifiedDate");
            DropColumn("dbo.PQEducations", "InfSuffRaiseBy");
            DropColumn("dbo.PQEducations", "InfSuffRaisedDate");
            DropColumn("dbo.PQEducations", "InfSuffClearBy");
            DropColumn("dbo.PQEducations", "InfSuffClearDate");
            DropColumn("dbo.PQEducations", "Mailto");
            DropColumn("dbo.PQEducations", "MailtoClient");
            DropColumn("dbo.PQEducations", "MailedBy");
            DropColumn("dbo.PQEducations", "ClientComment");
            DropColumn("dbo.PQEducations", "ClienINFClearDate");
            DropColumn("dbo.PQEducations", "INFRemarks");
            DropColumn("dbo.PQEducations", "StoppedCheckStatus");
            DropColumn("dbo.PQEducations", "StoppedCheckBy");
            DropColumn("dbo.PQEducations", "StoppedCheckDate");
            DropColumn("dbo.PQEducations", "StoppedCheckClearBy");
            DropColumn("dbo.PQEducations", "StoppedCheckClearDate");
            DropColumn("dbo.PQEducations", "ReportWritterBy");
            DropColumn("dbo.PQEducations", "ReportWritterDate");
            DropColumn("dbo.PQEducations", "RWQCManageBy");
            DropColumn("dbo.PQEducations", "RWQCManageDate");
            DropColumn("dbo.PQEducations", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQEducations", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQEducations", "RWQCTeamMemberBy");
            DropColumn("dbo.PQEducations", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQEducations", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQEducations", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEducations", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQEducations", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQEducations", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEducations", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEducations", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQEducations", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEducations", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEducations", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQEducations", "ReWorkCheckStatus");
        }
    }
}
