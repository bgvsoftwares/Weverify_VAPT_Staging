namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQCriminal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQCriminals", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQCriminals", "CaseStatus");
            DropColumn("dbo.PQCriminals", "MgrAllocatedBy");
            DropColumn("dbo.PQCriminals", "MgrAllocatedTo");
            DropColumn("dbo.PQCriminals", "MgrAllocatedDate");
            DropColumn("dbo.PQCriminals", "TLAllocatedBy");
            DropColumn("dbo.PQCriminals", "TLAllocatedTo");
            DropColumn("dbo.PQCriminals", "TLAllocatedDate");
            DropColumn("dbo.PQCriminals", "TMAllocatedBy");
            DropColumn("dbo.PQCriminals", "TMAllocatedTo");
            DropColumn("dbo.PQCriminals", "TMAllocatedDate");
            DropColumn("dbo.PQCriminals", "TMQCAllocatedBy");
            DropColumn("dbo.PQCriminals", "TMQCAllocatedTo");
            DropColumn("dbo.PQCriminals", "TMQCAllocatedDate");
            DropColumn("dbo.PQCriminals", "PVMgrAllocatedBy");
            DropColumn("dbo.PQCriminals", "PVMgrAllocatedTo");
            DropColumn("dbo.PQCriminals", "PVMgrAllocatedDate");
            DropColumn("dbo.PQCriminals", "PVTLAllocatedBy");
            DropColumn("dbo.PQCriminals", "PVTLAllocatedTo");
            DropColumn("dbo.PQCriminals", "PVTLAllocatedDate");
            DropColumn("dbo.PQCriminals", "PVTMAllocatedBy");
            DropColumn("dbo.PQCriminals", "PVTMAllocatedTo");
            DropColumn("dbo.PQCriminals", "PVTMAllocatedDate");
            DropColumn("dbo.PQCriminals", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQCriminals", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQCriminals", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQCriminals", "PVFVAllocatedBy");
            DropColumn("dbo.PQCriminals", "PVFVQAllocatedTo");
            DropColumn("dbo.PQCriminals", "PVFVAllocatedDate");
            DropColumn("dbo.PQCriminals", "Verify");
            DropColumn("dbo.PQCriminals", "VerifiedBy");
            DropColumn("dbo.PQCriminals", "VerifiedDate");
            DropColumn("dbo.PQCriminals", "InfSuffRaiseBy");
            DropColumn("dbo.PQCriminals", "InfSuffRaisedDate");
            DropColumn("dbo.PQCriminals", "InfSuffClearBy");
            DropColumn("dbo.PQCriminals", "InfSuffClearDate");
            DropColumn("dbo.PQCriminals", "Mailto");
            DropColumn("dbo.PQCriminals", "MailtoClient");
            DropColumn("dbo.PQCriminals", "MailedBy");
            DropColumn("dbo.PQCriminals", "ClientComment");
            DropColumn("dbo.PQCriminals", "ClienINFClearDate");
            DropColumn("dbo.PQCriminals", "INFRemarks");
            DropColumn("dbo.PQCriminals", "StoppedCheckStatus");
            DropColumn("dbo.PQCriminals", "StoppedCheckBy");
            DropColumn("dbo.PQCriminals", "StoppedCheckDate");
            DropColumn("dbo.PQCriminals", "StoppedCheckClearBy");
            DropColumn("dbo.PQCriminals", "StoppedCheckClearDate");
            DropColumn("dbo.PQCriminals", "ReportWritterBy");
            DropColumn("dbo.PQCriminals", "ReportWritterDate");
            DropColumn("dbo.PQCriminals", "RWQCManageBy");
            DropColumn("dbo.PQCriminals", "RWQCManageDate");
            DropColumn("dbo.PQCriminals", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQCriminals", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQCriminals", "RWQCTeamMemberBy");
            DropColumn("dbo.PQCriminals", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQCriminals", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQCriminals", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQCriminals", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQCriminals", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQCriminals", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQCriminals", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQCriminals", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQCriminals", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQCriminals", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQCriminals", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQCriminals", "ReWorkCheckStatus");
        }
    }
}
