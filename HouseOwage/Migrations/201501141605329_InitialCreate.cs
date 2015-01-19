namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentRequests",
                c => new
                    {
                        PaymentRequestId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy_UserId = c.Int(),
                        SentTo_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentRequestId)
                .ForeignKey("dbo.Users", t => t.CreatedBy_UserId)
                .ForeignKey("dbo.Users", t => t.SentTo_UserId)
                .Index(t => t.CreatedBy_UserId)
                .Index(t => t.SentTo_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Confirmed = c.Boolean(nullable: false),
                        PaymentRequest_PaymentRequestId = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.PaymentRequests", t => t.PaymentRequest_PaymentRequestId)
                .Index(t => t.PaymentRequest_PaymentRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "PaymentRequest_PaymentRequestId", "dbo.PaymentRequests");
            DropForeignKey("dbo.PaymentRequests", "SentTo_UserId", "dbo.Users");
            DropForeignKey("dbo.PaymentRequests", "CreatedBy_UserId", "dbo.Users");
            DropIndex("dbo.Payments", new[] { "PaymentRequest_PaymentRequestId" });
            DropIndex("dbo.PaymentRequests", new[] { "SentTo_UserId" });
            DropIndex("dbo.PaymentRequests", new[] { "CreatedBy_UserId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Users");
            DropTable("dbo.PaymentRequests");
        }
    }
}
