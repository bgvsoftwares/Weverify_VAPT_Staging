namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQEmployment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQEmployments", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQEmployments", "CaseStatus");
            DropColumn("dbo.PQEmployments", "MgrAllocatedBy");
            DropColumn("dbo.PQEmployments", "MgrAllocatedTo");
            DropColumn("dbo.PQEmployments", "MgrAllocatedDate");
            DropColumn("dbo.PQEmployments", "TLAllocatedBy");
            DropColumn("dbo.PQEmployments", "TLAllocatedTo");
            DropColumn("dbo.PQEmployments", "TLAllocatedDate");
            DropColumn("dbo.PQEmployments", "TMAllocatedBy");
            DropColumn("dbo.PQEmployments", "TMAllocatedTo");
            DropColumn("dbo.PQEmployments", "TMAllocatedDate");
            DropColumn("dbo.PQEmployments", "TMQCAllocatedBy");
            DropColumn("dbo.PQEmployments", "TMQCAllocatedTo");
            DropColumn("dbo.PQEmployments", "TMQCAllocatedDate");
            DropColumn("dbo.PQEmployments", "PVMgrAllocatedBy");
            DropColumn("dbo.PQEmployments", "PVMgrAllocatedTo");
            DropColumn("dbo.PQEmployments", "PVMgrAllocatedDate");
            DropColumn("dbo.PQEmployments", "PVTLAllocatedBy");
            DropColumn("dbo.PQEmployments", "PVTLAllocatedTo");
            DropColumn("dbo.PQEmployments", "PVTLAllocatedDate");
            DropColumn("dbo.PQEmployments", "PVTMAllocatedBy");
            DropColumn("dbo.PQEmployments", "PVTMAllocatedTo");
            DropColumn("dbo.PQEmployments", "PVTMAllocatedDate");
            DropColumn("dbo.PQEmployments", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQEmployments", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQEmployments", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQEmployments", "PVFVAllocatedBy");
            DropColumn("dbo.PQEmployments", "PVFVQAllocatedTo");
            DropColumn("dbo.PQEmployments", "PVFVAllocatedDate");
            DropColumn("dbo.PQEmployments", "Verify");
            DropColumn("dbo.PQEmployments", "VerifiedBy");
            DropColumn("dbo.PQEmployments", "VerifiedDate");
            DropColumn("dbo.PQEmployments", "InfSuffRaiseBy");
            DropColumn("dbo.PQEmployments", "InfSuffRaisedDate");
            DropColumn("dbo.PQEmployments", "InfSuffClearBy");
            DropColumn("dbo.PQEmployments", "InfSuffClearDate");
            DropColumn("dbo.PQEmployments", "Mailto");
            DropColumn("dbo.PQEmployments", "MailtoClient");
            DropColumn("dbo.PQEmployments", "MailedBy");
            DropColumn("dbo.PQEmployments", "ClientComment");
            DropColumn("dbo.PQEmployments", "ClienINFClearDate");
            DropColumn("dbo.PQEmployments", "INFRemarks");
            DropColumn("dbo.PQEmployments", "StoppedCheckStatus");
            DropColumn("dbo.PQEmployments", "StoppedCheckBy");
            DropColumn("dbo.PQEmployments", "StoppedCheckDate");
            DropColumn("dbo.PQEmployments", "StoppedCheckClearBy");
            DropColumn("dbo.PQEmployments", "StoppedCheckClearDate");
            DropColumn("dbo.PQEmployments", "ReportWritterBy");
            DropColumn("dbo.PQEmployments", "ReportWritterDate");
            DropColumn("dbo.PQEmployments", "RWQCManageBy");
            DropColumn("dbo.PQEmployments", "RWQCManageDate");
            DropColumn("dbo.PQEmployments", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQEmployments", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQEmployments", "RWQCTeamMemberBy");
            DropColumn("dbo.PQEmployments", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQEmployments", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQEmployments", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEmployments", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQEmployments", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQEmployments", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEmployments", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQEmployments", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQEmployments", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQEmployments", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQEmployments", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQEmployments", "ReWorkCheckStatus");
        }
    }
}
