namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalPaymentRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalPaymentRequests",
                c => new
                    {
                        ExternalPaymentRequestId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestTo = c.String(),
                        Created = c.DateTime(nullable: false),
                        Name = c.String(),
                        Paid = c.Boolean(nullable: false),
                        CreatedBy_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ExternalPaymentRequestId)
                .ForeignKey("dbo.Users", t => t.CreatedBy_UserId)
                .Index(t => t.CreatedBy_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExternalPaymentRequests", "CreatedBy_UserId", "dbo.Users");
            DropIndex("dbo.ExternalPaymentRequests", new[] { "CreatedBy_UserId" });
            DropTable("dbo.ExternalPaymentRequests");
        }
    }
}
