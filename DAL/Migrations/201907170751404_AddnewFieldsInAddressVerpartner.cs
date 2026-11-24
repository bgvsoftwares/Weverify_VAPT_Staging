namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnewFieldsInAddressVerpartner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQAddressVarParteners", "SecurityConfirm", c => c.String());
            AddColumn("dbo.PQAddressVarParteners", "SocietyBoard", c => c.String());
            AddColumn("dbo.PQAddressVarParteners", "Photo_Taken", c => c.String());
            AddColumn("dbo.PQAddressVarParteners", "AgencyExecutive_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQAddressVarParteners", "AgencyExecutive_Name");
            DropColumn("dbo.PQAddressVarParteners", "Photo_Taken");
            DropColumn("dbo.PQAddressVarParteners", "SocietyBoard");
            DropColumn("dbo.PQAddressVarParteners", "SecurityConfirm");
        }
    }
}
