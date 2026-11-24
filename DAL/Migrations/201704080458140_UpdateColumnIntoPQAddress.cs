namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQAddresses", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQAddresses", "CaseStatus");
            DropColumn("dbo.PQAddresses", "MgrAllocatedBy");
            DropColumn("dbo.PQAddresses", "MgrAllocatedTo");
            DropColumn("dbo.PQAddresses", "MgrAllocatedDate");
            DropColumn("dbo.PQAddresses", "TLAllocatedBy");
            DropColumn("dbo.PQAddresses", "TLAllocatedTo");
            DropColumn("dbo.PQAddresses", "TLAllocatedDate");
            DropColumn("dbo.PQAddresses", "TMAllocatedBy");
            DropColumn("dbo.PQAddresses", "TMAllocatedTo");
            DropColumn("dbo.PQAddresses", "TMAllocatedDate");
            DropColumn("dbo.PQAddresses", "TMQCAllocatedBy");
            DropColumn("dbo.PQAddresses", "TMQCAllocatedTo");
            DropColumn("dbo.PQAddresses", "TMQCAllocatedDate");
            DropColumn("dbo.PQAddresses", "PVMgrAllocatedBy");
            DropColumn("dbo.PQAddresses", "PVMgrAllocatedTo");
            DropColumn("dbo.PQAddresses", "PVMgrAllocatedDate");
            DropColumn("dbo.PQAddresses", "PVTLAllocatedBy");
            DropColumn("dbo.PQAddresses", "PVTLAllocatedTo");
            DropColumn("dbo.PQAddresses", "PVTLAllocatedDate");
            DropColumn("dbo.PQAddresses", "PVTMAllocatedBy");
            DropColumn("dbo.PQAddresses", "PVTMAllocatedTo");
            DropColumn("dbo.PQAddresses", "PVTMAllocatedDate");
            DropColumn("dbo.PQAddresses", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQAddresses", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQAddresses", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQAddresses", "PVFVAllocatedBy");
            DropColumn("dbo.PQAddresses", "PVFVQAllocatedTo");
            DropColumn("dbo.PQAddresses", "PVFVAllocatedDate");
            DropColumn("dbo.PQAddresses", "Verify");
            DropColumn("dbo.PQAddresses", "VerifiedBy");
            DropColumn("dbo.PQAddresses", "VerifiedDate");
            DropColumn("dbo.PQAddresses", "InfSuffRaiseBy");
            DropColumn("dbo.PQAddresses", "InfSuffRaisedDate");
            DropColumn("dbo.PQAddresses", "InfSuffClearBy");
            DropColumn("dbo.PQAddresses", "InfSuffClearDate");
            DropColumn("dbo.PQAddresses", "Mailto");
            DropColumn("dbo.PQAddresses", "MailtoClient");
            DropColumn("dbo.PQAddresses", "MailedBy");
            DropColumn("dbo.PQAddresses", "ClientComment");
            DropColumn("dbo.PQAddresses", "ClienINFClearDate");
            DropColumn("dbo.PQAddresses", "INFRemarks");
            DropColumn("dbo.PQAddresses", "StoppedCheckStatus");
            DropColumn("dbo.PQAddresses", "StoppedCheckBy");
            DropColumn("dbo.PQAddresses", "StoppedCheckDate");
            DropColumn("dbo.PQAddresses", "StoppedCheckClearBy");
            DropColumn("dbo.PQAddresses", "StoppedCheckClearDate");
            DropColumn("dbo.PQAddresses", "ReportWritterBy");
            DropColumn("dbo.PQAddresses", "ReportWritterDate");
            DropColumn("dbo.PQAddresses", "RWQCManageBy");
            DropColumn("dbo.PQAddresses", "RWQCManageDate");
            DropColumn("dbo.PQAddresses", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQAddresses", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQAddresses", "RWQCTeamMemberBy");
            DropColumn("dbo.PQAddresses", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQAddresses", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQAddresses", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQAddresses", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQAddresses", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQAddresses", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQAddresses", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQAddresses", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQAddresses", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQAddresses", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQAddresses", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQAddresses", "ReWorkCheckStatus");
        }
    }
}
