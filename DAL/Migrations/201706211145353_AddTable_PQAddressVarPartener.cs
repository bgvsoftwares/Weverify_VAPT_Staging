namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQAddressVarPartener : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQAddressVarParteners",
                c => new
                    {
                        PQAddressVarPtrRowId = c.Int(nullable: false, identity: true),
                        AddressRowID = c.Int(nullable: false),
                        VendorLoginRowID = c.Int(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        SubCheckRowId = c.Int(nullable: false),
                        SubCheckName = c.String(maxLength: 100),
                        AV_Cand_Name = c.String(maxLength: 100),
                        AV_Add = c.String(maxLength: 100),
                        AV_AddLine1 = c.String(maxLength: 100),
                        AV_AddLine2 = c.String(maxLength: 100),
                        AV_Buldng_Street_Name = c.String(maxLength: 100),
                        AV_State = c.String(maxLength: 100),
                        AV_District = c.String(maxLength: 100),
                        AV_City = c.String(maxLength: 100),
                        AV_Pincode = c.Int(nullable: false),
                        AV_Landmark = c.String(maxLength: 100),
                        InternalOutDate = c.DateTime(),
                        CheckStatus = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(),
                        IsSynced = c.Boolean(nullable: false),
                        SyncedBy = c.Int(nullable: false),
                        SyncedOn = c.DateTime(),
                        IsAddressChanged = c.String(maxLength: 100),
                        Address = c.String(maxLength: 100),
                        DurationOfStay = c.String(maxLength: 100),
                        AccomodationType = c.String(maxLength: 100),
                        RespName = c.String(maxLength: 100),
                        RespIdDetails = c.String(maxLength: 100),
                        RespRelation = c.String(maxLength: 100),
                        RespContactDetails = c.String(maxLength: 100),
                        NeighbourNameAndNos = c.String(maxLength: 100),
                        AnyOtherComments = c.String(maxLength: 200),
                        PtrStatus = c.String(maxLength: 100),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Longitude = c.String(maxLength: 100),
                        Latitude = c.String(maxLength: 100),
                        IsMSynced = c.Boolean(nullable: false),
                        MSyncedBy = c.Int(nullable: false),
                        MSyncedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.PQAddressVarPtrRowId)
                .ForeignKey("dbo.MasterVendorLogins", t => t.VendorLoginRowID)
                .ForeignKey("dbo.PQAddresses", t => t.AddressRowID)
                .Index(t => t.AddressRowID)
                .Index(t => t.VendorLoginRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQAddressVarParteners", "AddressRowID", "dbo.PQAddresses");
            DropForeignKey("dbo.PQAddressVarParteners", "VendorLoginRowID", "dbo.MasterVendorLogins");
            DropIndex("dbo.PQAddressVarParteners", new[] { "VendorLoginRowID" });
            DropIndex("dbo.PQAddressVarParteners", new[] { "AddressRowID" });
            DropTable("dbo.PQAddressVarParteners");
        }
    }
}
