namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentRequests", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentRequests", "Name");
        }
    }
}
