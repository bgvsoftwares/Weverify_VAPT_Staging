namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQNationalIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQNationalIdentities",
                c => new
                    {
                        NationalIdentityRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        NIC_Cand_Name = c.String(maxLength: 100),
                        NIC_Sec_Ref_Id = c.String(maxLength: 50),
                        NIC_DOB = c.DateTime(),
                        NIC_DOB1 = c.DateTime(),
                        NIC_ID_No = c.String(maxLength: 50),
                        NIC_Date_Issue = c.DateTime(),
                        NIC_Expiry_Date = c.DateTime(),
                        NIC_Passport_No = c.String(maxLength: 20),
                        NIC_SSN_NO = c.String(maxLength: 50),
                        NIC_Pan_No = c.String(maxLength: 20),
                        NIC_Voter_Id_No = c.String(maxLength: 20),
                        NIC_Dvng_Liscense_No = c.String(maxLength: 20),
                        NIC_Aadhar_Card_No = c.String(maxLength: 20),
                        NIC_Place_Issue = c.String(maxLength: 100),
                        NIC_Place_Residnce = c.String(maxLength: 100),
                        NIC_Type_Id_Card = c.String(maxLength: 100),
                        NIC_Reg_Doc_Type = c.String(maxLength: 100),
                        NIC_RegNo = c.String(maxLength: 100),
                        NIC_PinCode = c.String(maxLength: 10),
                        NIC_Name_Key_Personnel = c.String(maxLength: 100),
                        NIC_AliasFname = c.String(maxLength: 50),
                        NIC_Alias_Lname = c.String(maxLength: 50),
                        NIC_Country_Iss_Passport = c.String(maxLength: 100),
                        NIC_Gender = c.String(maxLength: 20),
                        NIC_Id_CardNo = c.String(maxLength: 50),
                        NIC_Identity_Prov = c.String(maxLength: 100),
                        NIC_Name_Changed_Effect_Frm = c.DateTime(),
                        NIC_Nationality = c.String(maxLength: 100),
                        NIC_AadhaarCard = c.Byte(nullable: false),
                        NIC_DrivingLicense = c.Byte(nullable: false),
                        NIC_PANCard = c.Byte(nullable: false),
                        NIC_Passport = c.Byte(nullable: false),
                        NIC_VoterID = c.Byte(nullable: false),
                        NIC_OtherProof = c.Byte(nullable: false),
                        NIC_OtherDetails = c.String(maxLength: 200),
                        NIC_OtherDetails2 = c.String(maxLength: 200),
                        NIC_OtherDetails3 = c.String(maxLength: 200),
                        NIC_OtherDetails4 = c.String(maxLength: 200),
                        NIC_OtherDetails5 = c.String(maxLength: 200),
                        NIC_OtherDetails6 = c.String(maxLength: 200),
                        NIC_OtherDetails7 = c.String(maxLength: 200),
                        NIC_OtherDetails8 = c.String(maxLength: 200),
                        NIC_OtherDetails9 = c.String(maxLength: 200),
                        NIC_OtherDetails10 = c.String(maxLength: 200),
                        ATA_CID_No = c.String(maxLength: 100),
                        ATA_Cmpny_Addr = c.String(maxLength: 100),
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
                .PrimaryKey(t => t.NationalIdentityRowID)
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
            DropForeignKey("dbo.PQNationalIdentities", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQNationalIdentities", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQNationalIdentities", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQNationalIdentities", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQNationalIdentities", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQNationalIdentities", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQNationalIdentities", new[] { "PersonalRowID" });
            DropIndex("dbo.PQNationalIdentities", new[] { "ClientRowID" });
            DropTable("dbo.PQNationalIdentities");
        }
    }
}
