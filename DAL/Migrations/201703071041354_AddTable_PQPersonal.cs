namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQPersonal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQPersonals",
                c => new
                    {
                        PersonalRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        ClientUserRowID = c.Int(nullable: false),
                        CandidateUserID = c.Int(nullable: false),
                        ClientRefID = c.String(maxLength: 100),
                        CompanyRefNo = c.String(maxLength: 100),
                        CandidateCode = c.String(maxLength: 100),
                        CandTitle = c.String(nullable: false, maxLength: 15),
                        Name = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        FatherTitle = c.String(maxLength: 15),
                        FatherName = c.String(maxLength: 50),
                        FatherMiddleName = c.String(maxLength: 50),
                        FatherLastName = c.String(maxLength: 50),
                        Gender = c.String(nullable: false, maxLength: 10),
                        DOB = c.DateTime(),
                        MaritalStatus = c.String(maxLength: 15),
                        MobileNo = c.String(maxLength: 20),
                        AlternateNo = c.String(maxLength: 20),
                        EmailID = c.String(maxLength: 100),
                        AlternateEmailID = c.String(maxLength: 100),
                        STD = c.String(maxLength: 10),
                        ResidencePhoneNo = c.String(maxLength: 20),
                        PanCard = c.Byte(nullable: false),
                        PanCardNo = c.String(maxLength: 100),
                        Passport = c.Byte(nullable: false),
                        PassportNo = c.String(maxLength: 100),
                        AadhaarCard = c.Byte(nullable: false),
                        AadhaarCardNo = c.String(maxLength: 100),
                        DrivingLicense = c.Byte(nullable: false),
                        DrivingLicenseNo = c.String(maxLength: 100),
                        VoterIDCard = c.Byte(nullable: false),
                        VoterIDCardNo = c.String(maxLength: 100),
                        CheckDateTime = c.DateTime(),
                        CandidateType = c.String(maxLength: 50),
                        Catagory = c.String(maxLength: 50),
                        CaseType = c.String(maxLength: 20),
                        TAT = c.Byte(nullable: false),
                        Nationality = c.String(maxLength: 50),
                        EmployeeID = c.String(maxLength: 100),
                        Designation = c.String(maxLength: 100),
                        PlaceofJoining = c.String(maxLength: 100),
                        DateofJoining = c.DateTime(),
                        NationalSkills = c.String(maxLength: 100),
                        SSNNo = c.String(maxLength: 50),
                        FromType = c.String(maxLength: 50),
                        DetailsEnteredBy = c.String(maxLength: 50),
                        ProcessName = c.String(maxLength: 70),
                        BusinessName = c.String(maxLength: 50),
                        Type = c.String(maxLength: 50),
                        SpouseName = c.String(maxLength: 80),
                        SpouseMiddleName = c.String(maxLength: 80),
                        SpouseLastName = c.String(maxLength: 80),
                        OrderDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        CreatedBy = c.Short(nullable: false),
                        CreatedDate = c.DateTime(),
                        AllocatedBy = c.Short(nullable: false),
                        AllocatedToDEScan = c.Short(nullable: false),
                        AllocatedToDEScanDate = c.DateTime(),
                        AllocatedToDE = c.Short(nullable: false),
                        AllocatedToDEDate = c.DateTime(),
                        AllocatedToDEQC = c.Short(nullable: false),
                        AllocatedToDEQCDate = c.DateTime(),
                        DocUploaded = c.Byte(nullable: false),
                        CaseStatus = c.String(maxLength: 30),
                        FinalStatus = c.String(maxLength: 30),
                        FinalColor = c.String(maxLength: 50),
                        ExitIntrvwApplicable = c.Byte(nullable: false),
                        Remark = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                        OtherDetails = c.String(maxLength: 100),
                        OtherDetails1 = c.String(maxLength: 100),
                        OtherDetails2 = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PersonalRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQPersonals", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQPersonals", new[] { "ClientRowID" });
            DropTable("dbo.PQPersonals");
        }
    }
}
