namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentArchiveCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentRequests", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payments", "Archived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Created");
            DropColumn("dbo.Payments", "Archived");
            DropColumn("dbo.PaymentRequests", "Created");
        }
    }
}
