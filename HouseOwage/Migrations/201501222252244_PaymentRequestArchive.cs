namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentRequestArchive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentRequests", "Archived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentRequests", "Archived");
        }
    }
}
