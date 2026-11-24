namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQAddresses",
                c => new
                    {
                        AddressRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        AV_Cand_Name = c.String(maxLength: 100),
                        AV_Sec_Ref_No = c.String(maxLength: 50),
                        AV_Add = c.String(maxLength: 200),
                        AV_Add_Line1 = c.String(maxLength: 200),
                        AV_Add_Line2 = c.String(maxLength: 200),
                        AV_Buldng_Street_name = c.String(maxLength: 100),
                        AV_State = c.Short(nullable: false),
                        AV_District = c.Short(nullable: false),
                        AV_City = c.Int(nullable: false),
                        AV_Pincode = c.Int(nullable: false),
                        AV_Landmark = c.String(maxLength: 100),
                        AV_Add_Type = c.String(maxLength: 100),
                        AV_Dur_of_Stay = c.String(maxLength: 100),
                        AV_Owner_Name = c.String(maxLength: 100),
                        AV_Owner_ConNo = c.String(maxLength: 20),
                        AV_NickName = c.String(maxLength: 50),
                        AV_Name_Chngd_Effect_Frm = c.String(maxLength: 50),
                        AV_Doc_Prov_Proof_Add = c.String(maxLength: 200),
                        AddressProofFileName = c.String(maxLength: 200),
                        AV_Doc_Details = c.String(maxLength: 200),
                        DocumentDetailsFileName = c.String(maxLength: 200),
                        AV_RationCard = c.Byte(nullable: false),
                        AV_TelephoneBill = c.Byte(nullable: false),
                        AV_RentAgreement = c.Byte(nullable: false),
                        AV_Passport = c.Byte(nullable: false),
                        AV_ElectricityBill = c.Byte(nullable: false),
                        AV_GasSupplyBill = c.Byte(nullable: false),
                        AV_WaterBill = c.Byte(nullable: false),
                        AV_OtherProof = c.Byte(nullable: false),
                        AV_OtherDetails = c.String(maxLength: 200),
                        AV_OtherDetails2 = c.String(maxLength: 200),
                        AV_OtherDetails3 = c.String(maxLength: 200),
                        AV_OtherDetails4 = c.String(maxLength: 200),
                        AV_OtherDetails5 = c.String(maxLength: 200),
                        AV_OtherDetails6 = c.String(maxLength: 200),
                        AV_OtherDetails7 = c.String(maxLength: 200),
                        AV_OtherDetails8 = c.String(maxLength: 200),
                        AV_OtherDetails9 = c.String(maxLength: 200),
                        AV_OtherDetails10 = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.AddressRowID)
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
            DropForeignKey("dbo.PQAddresses", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQAddresses", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQAddresses", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQAddresses", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQAddresses", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQAddresses", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQAddresses", new[] { "PersonalRowID" });
            DropIndex("dbo.PQAddresses", new[] { "ClientRowID" });
            DropTable("dbo.PQAddresses");
        }
    }
}
