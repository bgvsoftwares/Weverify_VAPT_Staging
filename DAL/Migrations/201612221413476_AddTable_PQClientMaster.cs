namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientMasters",
                c => new
                    {
                        ClientRowID = c.Short(nullable: false, identity: true),
                        ClientCode = c.String(nullable: false, maxLength: 50),
                        ClientCode1 = c.String(maxLength: 20),
                        ClientAbbRowID = c.Short(nullable: false),
                        ClientSubgroupID = c.Short(nullable: false),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        PinCode = c.Int(nullable: false),
                        Address = c.String(maxLength: 300),
                        RegisteredAddress = c.String(maxLength: 300),
                        CorporateOfficeAddress = c.String(maxLength: 300),
                        CodeGeneration = c.Byte(nullable: false),
                        BORowID = c.Short(nullable: false),
                        BillingRowID = c.Byte(nullable: false),
                        ReportSentBy = c.Byte(nullable: false),
                        InterimReport = c.Byte(nullable: false),
                        ContractDate = c.DateTime(),
                        ContractComplitionDate = c.DateTime(),
                        PricingType = c.Byte(nullable: false),
                        PaymentTermIndays = c.Byte(nullable: false),
                        SpecialInstructions = c.String(maxLength: 200),
                        MapAntecedent = c.Byte(nullable: false),
                        MapDisposition = c.Byte(nullable: false),
                        MapSeverity = c.Byte(nullable: false),
                        MapHolidays = c.Byte(nullable: false),
                        MailSendBy = c.Byte(nullable: false),
                        HRApprovalRequired = c.Byte(nullable: false),
                        DaysCalculation = c.Byte(nullable: false),
                        ReOpenCases = c.Byte(nullable: false),
                        ReOpenBilling = c.Byte(nullable: false),
                        InsuffBilling = c.Byte(nullable: false),
                        OtherThanGreenBilling = c.Byte(nullable: false),
                        StopCaseBilling = c.Byte(nullable: false),
                        WIPOrClosedBilling = c.Byte(nullable: false),
                        POApplicable = c.Byte(nullable: false),
                        EmploymentPV = c.Byte(nullable: false),
                        YTRStatus = c.Byte(nullable: false),
                        VerbalReportStatus = c.Byte(nullable: false),
                        CandidateContact = c.Byte(nullable: false),
                        ClientNameDisclosure = c.Byte(nullable: false),
                        ClientStatus = c.Byte(nullable: false),
                        ExtraExpenses = c.Byte(nullable: false),
                        EEAllowedAmount = c.Double(nullable: false),
                        BusinessCommitment = c.Byte(nullable: false),
                        BusinsComtntNoOfCase = c.Short(nullable: false),
                        InsuffClientReport = c.Byte(nullable: false),
                        InsuffHoldDays = c.Byte(nullable: false),
                        Incentive = c.Byte(nullable: false),
                        IncentiveInstruction = c.String(maxLength: 100),
                        Penalty = c.Byte(nullable: false),
                        PenaltyDetails = c.String(maxLength: 100),
                        Liability = c.Byte(nullable: false),
                        LiabilityDetails = c.String(maxLength: 100),
                        BillingAprvl = c.Byte(nullable: false),
                        BillingAprvlDetails = c.String(maxLength: 100),
                        SpocName = c.String(nullable: false, maxLength: 3000),
                        SpocEmailID = c.String(nullable: false),
                        CSpocName = c.String(nullable: false, maxLength: 100),
                        CSpocDesignation = c.String(maxLength: 100),
                        CSpocContactNo = c.String(maxLength: 20),
                        CSpocMobileNo = c.String(maxLength: 15),
                        CSpocEmailID = c.String(nullable: false, maxLength: 200),
                        CBillingSpocName = c.String(maxLength: 100),
                        CBillingSpocDesignation = c.String(maxLength: 100),
                        CBillingSpocMobileNo = c.String(maxLength: 15),
                        CBillingSpocEmailID = c.String(maxLength: 200),
                        CBillingInstructions = c.String(maxLength: 200),
                        CBillingAddress = c.String(maxLength: 300),
                        CEsclationSpocName = c.String(maxLength: 100),
                        CEsclationSpocDesignation = c.String(maxLength: 100),
                        CEsclationSpocMobileNo = c.String(maxLength: 15),
                        CEsclationSpocEmailID = c.String(maxLength: 200),
                        CSendInsuffDisplay = c.String(maxLength: 100),
                        CSendInsuffEmail = c.String(maxLength: 100),
                        CSendReportDisplay = c.String(maxLength: 100),
                        CSendReportEmail = c.String(maxLength: 100),
                        CSendRedReportDisplay = c.String(maxLength: 100),
                        CSendRedReportEmail = c.String(maxLength: 100),
                        CSendBillingAprvlDisplay = c.String(maxLength: 100),
                        CSendBillingAprvlEmail = c.String(maxLength: 100),
                        Remark = c.String(maxLength: 500),
                        SMTPServer = c.String(maxLength: 100),
                        SMTPPort = c.String(maxLength: 5),
                        SMTPUserName = c.String(maxLength: 100),
                        SMTPPassword = c.String(maxLength: 100),
                        EnableSsl = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreatedDate = c.DateTime(),
                        SLAUploaded = c.Byte(nullable: false),
                        CoverPage = c.Byte(nullable: false),
                        AgreementType = c.Byte(nullable: false),
                        PrevClientRowID = c.Short(nullable: false),
                        HardCopy = c.Byte(nullable: false),
                        ClientCategory = c.String(maxLength: 50),
                        CSalesSPOCName = c.String(maxLength: 100),
                        CSalesStatus = c.Byte(nullable: false),
                        CSalesCount = c.Short(nullable: false),
                        Other1 = c.String(maxLength: 100),
                        Other2 = c.String(maxLength: 100),
                        Other3 = c.String(maxLength: 100),
                        Other4 = c.String(maxLength: 100),
                        Other5 = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ClientRowID)
                .ForeignKey("dbo.MasterClientAbbreviations", t => t.ClientAbbRowID)
                .ForeignKey("dbo.MasterBillingCycles", t => t.BillingRowID)
                .ForeignKey("dbo.MasterClientSubGroups", t => t.ClientSubgroupID)
                .ForeignKey("dbo.MasterCompanyBranches", t => t.BORowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterDistricts", t => t.DistrictRowID)
                .ForeignKey("dbo.MasterLocations", t => t.LocationRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .Index(t => t.ClientAbbRowID)
                .Index(t => t.ClientSubgroupID)
                .Index(t => t.CountryRowID)
                .Index(t => t.StateRowID)
                .Index(t => t.DistrictRowID)
                .Index(t => t.LocationRowID)
                .Index(t => t.BORowID)
                .Index(t => t.BillingRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientMasters", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.PQClientMasters", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.PQClientMasters", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.PQClientMasters", "CountryRowID", "dbo.MasterCountries");
            DropForeignKey("dbo.PQClientMasters", "BORowID", "dbo.MasterCompanyBranches");
            DropForeignKey("dbo.PQClientMasters", "ClientSubgroupID", "dbo.MasterClientSubGroups");
            DropForeignKey("dbo.PQClientMasters", "BillingRowID", "dbo.MasterBillingCycles");
            DropForeignKey("dbo.PQClientMasters", "ClientAbbRowID", "dbo.MasterClientAbbreviations");
            DropIndex("dbo.PQClientMasters", new[] { "BillingRowID" });
            DropIndex("dbo.PQClientMasters", new[] { "BORowID" });
            DropIndex("dbo.PQClientMasters", new[] { "LocationRowID" });
            DropIndex("dbo.PQClientMasters", new[] { "DistrictRowID" });
            DropIndex("dbo.PQClientMasters", new[] { "StateRowID" });
            DropIndex("dbo.PQClientMasters", new[] { "CountryRowID" });
            DropIndex("dbo.PQClientMasters", new[] { "ClientSubgroupID" });
            DropIndex("dbo.PQClientMasters", new[] { "ClientAbbRowID" });
            DropTable("dbo.PQClientMasters");
        }
    }
}
