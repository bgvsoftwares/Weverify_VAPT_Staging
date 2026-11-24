namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIntoPQAddressVarPartener : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQAddressVarParteners", "Landmark", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "CheckStatus", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQAddressVarParteners", "IsAddressChanged", c => c.String(maxLength: 5));
            AlterColumn("dbo.PQAddressVarParteners", "Address", c => c.String(maxLength: 300));
            AlterColumn("dbo.PQAddressVarParteners", "DurationOfStay", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQAddressVarParteners", "PtrStatus", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQAddressVarParteners", "Longitude", c => c.String(maxLength: 30));
            AlterColumn("dbo.PQAddressVarParteners", "Latitude", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQAddressVarParteners", "Latitude", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "Longitude", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "PtrStatus", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "DurationOfStay", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "Address", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "IsAddressChanged", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQAddressVarParteners", "CheckStatus", c => c.String(maxLength: 100));
            DropColumn("dbo.PQAddressVarParteners", "Landmark");
        }
    }
}
