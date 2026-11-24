namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQReference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQReferences",
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
                        ModifiedBy = c.Short(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ReferenceRowID)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.TempPQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPQReferences", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQReferences", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQReferences", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQReferences", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQReferences", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQReferences", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQReferences", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQReferences", new[] { "ClientRowID" });
            DropTable("dbo.TempPQReferences");
        }
    }
}
