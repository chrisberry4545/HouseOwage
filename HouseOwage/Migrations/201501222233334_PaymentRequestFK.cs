namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentRequestFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentRequests", "SentTo_UserId", "dbo.Users");
            DropIndex("dbo.PaymentRequests", new[] { "SentTo_UserId" });
            AlterColumn("dbo.PaymentRequests", "SentTo_UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.PaymentRequests", "SentTo_UserId");
            AddForeignKey("dbo.PaymentRequests", "SentTo_UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentRequests", "SentTo_UserId", "dbo.Users");
            DropIndex("dbo.PaymentRequests", new[] { "SentTo_UserId" });
            AlterColumn("dbo.PaymentRequests", "SentTo_UserId", c => c.Int());
            CreateIndex("dbo.PaymentRequests", "SentTo_UserId");
            AddForeignKey("dbo.PaymentRequests", "SentTo_UserId", "dbo.Users", "UserId");
        }
    }
}
