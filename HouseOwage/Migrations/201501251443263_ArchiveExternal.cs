namespace HouseOwage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchiveExternal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExternalPaymentRequests", "Archived", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ExternalPaymentRequests", "RequestTo", c => c.String(nullable: false));
            AlterColumn("dbo.ExternalPaymentRequests", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExternalPaymentRequests", "Name", c => c.String());
            AlterColumn("dbo.ExternalPaymentRequests", "RequestTo", c => c.String());
            DropColumn("dbo.ExternalPaymentRequests", "Archived");
        }
    }
}
