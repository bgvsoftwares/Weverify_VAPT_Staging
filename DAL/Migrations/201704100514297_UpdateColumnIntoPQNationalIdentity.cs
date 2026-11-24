namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQNationalIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQNationalIdentities", "ReWorkCheckStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQNationalIdentities", "CaseStatus");
            DropColumn("dbo.PQNationalIdentities", "MgrAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "MgrAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "MgrAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "TLAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "TLAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "TLAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "TMAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "TMAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "TMAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "TMQCAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "TMQCAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "TMQCAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "PVMgrAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "PVMgrAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "PVMgrAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "PVTLAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "PVTLAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "PVTLAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "PVTMAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "PVTMAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "PVTMAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "PVFVAllocatedBy");
            DropColumn("dbo.PQNationalIdentities", "PVFVQAllocatedTo");
            DropColumn("dbo.PQNationalIdentities", "PVFVAllocatedDate");
            DropColumn("dbo.PQNationalIdentities", "Verify");
            DropColumn("dbo.PQNationalIdentities", "VerifiedBy");
            DropColumn("dbo.PQNationalIdentities", "VerifiedDate");
            DropColumn("dbo.PQNationalIdentities", "InfSuffRaiseBy");
            DropColumn("dbo.PQNationalIdentities", "InfSuffRaisedDate");
            DropColumn("dbo.PQNationalIdentities", "InfSuffClearBy");
            DropColumn("dbo.PQNationalIdentities", "InfSuffClearDate");
            DropColumn("dbo.PQNationalIdentities", "Mailto");
            DropColumn("dbo.PQNationalIdentities", "MailtoClient");
            DropColumn("dbo.PQNationalIdentities", "MailedBy");
            DropColumn("dbo.PQNationalIdentities", "ClientComment");
            DropColumn("dbo.PQNationalIdentities", "ClienINFClearDate");
            DropColumn("dbo.PQNationalIdentities", "INFRemarks");
            DropColumn("dbo.PQNationalIdentities", "StoppedCheckStatus");
            DropColumn("dbo.PQNationalIdentities", "StoppedCheckBy");
            DropColumn("dbo.PQNationalIdentities", "StoppedCheckDate");
            DropColumn("dbo.PQNationalIdentities", "StoppedCheckClearBy");
            DropColumn("dbo.PQNationalIdentities", "StoppedCheckClearDate");
            DropColumn("dbo.PQNationalIdentities", "ReportWritterBy");
            DropColumn("dbo.PQNationalIdentities", "ReportWritterDate");
            DropColumn("dbo.PQNationalIdentities", "RWQCManageBy");
            DropColumn("dbo.PQNationalIdentities", "RWQCManageDate");
            DropColumn("dbo.PQNationalIdentities", "RWQCTeamLeaderBy");
            DropColumn("dbo.PQNationalIdentities", "RWQCTeamLeaderDate");
            DropColumn("dbo.PQNationalIdentities", "RWQCTeamMemberBy");
            DropColumn("dbo.PQNationalIdentities", "RWQCTeamMemberDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQNationalIdentities", "RWQCTeamMemberDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "RWQCTeamMemberBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "RWQCTeamLeaderDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "RWQCTeamLeaderBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "RWQCManageDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "RWQCManageBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "ReportWritterDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "ReportWritterBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "StoppedCheckClearDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "StoppedCheckClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "StoppedCheckDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "StoppedCheckBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "StoppedCheckStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "INFRemarks", c => c.String(maxLength: 200));
            AddColumn("dbo.PQNationalIdentities", "ClienINFClearDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "ClientComment", c => c.String(maxLength: 100));
            AddColumn("dbo.PQNationalIdentities", "MailedBy", c => c.String(maxLength: 100));
            AddColumn("dbo.PQNationalIdentities", "MailtoClient", c => c.String(maxLength: 200));
            AddColumn("dbo.PQNationalIdentities", "Mailto", c => c.String(maxLength: 200));
            AddColumn("dbo.PQNationalIdentities", "InfSuffClearDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "InfSuffClearBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "InfSuffRaisedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "InfSuffRaiseBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "VerifiedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "VerifiedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "Verify", c => c.Byte(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVFVAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "PVFVQAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVFVAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "PVTMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "PVTLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVTLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVMgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "PVMgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "PVMgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TMQCAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "TMQCAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TMQCAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TMAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "TMAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TMAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TLAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "TLAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "TLAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "MgrAllocatedDate", c => c.DateTime());
            AddColumn("dbo.PQNationalIdentities", "MgrAllocatedTo", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "MgrAllocatedBy", c => c.Short(nullable: false));
            AddColumn("dbo.PQNationalIdentities", "CaseStatus", c => c.String(maxLength: 20));
            DropColumn("dbo.PQNationalIdentities", "ReWorkCheckStatus");
        }
    }
}
