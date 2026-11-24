namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQEducation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQEducations",
                c => new
                    {
                        EducationRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 20),
                        EV_Cand_Name = c.String(nullable: false, maxLength: 100),
                        EV_Sec_Ref_Id = c.String(maxLength: 50),
                        EV_Cand_Name_Supp_Edu_Doc = c.String(maxLength: 100),
                        EV_Colg_Schl_Inst_Name = c.Int(nullable: false),
                        EV_Colg_Schl_Inst_Add_ConDet = c.String(maxLength: 200),
                        EV_Affl_University_Name = c.Short(nullable: false),
                        EV_Degree_Name = c.Short(nullable: false),
                        EV_Mode_Of_Edu = c.String(maxLength: 50),
                        EV_Enroll_RollNo = c.String(maxLength: 50),
                        EV_RegNo_RegCode = c.String(maxLength: 50),
                        EV_Dur_Course = c.String(maxLength: 50),
                        EV_Course_Status = c.String(maxLength: 50),
                        EV_Specialization = c.String(maxLength: 100),
                        EV_Period_Course = c.String(maxLength: 100),
                        EV_Yr_Passing = c.String(maxLength: 10),
                        EV_Division_CGPA = c.String(maxLength: 50),
                        EV_DegreeCertfict = c.Byte(nullable: false),
                        EV_ProvisionalCertfict = c.Byte(nullable: false),
                        EV_GradeSheet = c.Byte(nullable: false),
                        EV_AllMarksCards = c.Byte(nullable: false),
                        EV_DocStatColgNameNo = c.Byte(nullable: false),
                        EV_OtherProof = c.Byte(nullable: false),
                        EV_Others = c.String(maxLength: 200),
                        EV_Others2 = c.String(maxLength: 200),
                        EV_Others3 = c.String(maxLength: 200),
                        EV_Others4 = c.String(maxLength: 200),
                        EV_Others5 = c.String(maxLength: 200),
                        EV_Others6 = c.String(maxLength: 200),
                        EV_Others7 = c.String(maxLength: 200),
                        EV_Others8 = c.String(maxLength: 200),
                        EV_Others9 = c.String(maxLength: 200),
                        EV_Others10 = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.EducationRowId)
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
            DropForeignKey("dbo.TempPQEducations", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQEducations", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.TempPQEducations", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.TempPQEducations", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.TempPQEducations", new[] { "SubCheckRowID" });
            DropIndex("dbo.TempPQEducations", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.TempPQEducations", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQEducations", new[] { "ClientRowID" });
            DropTable("dbo.TempPQEducations");
        }
    }
}
