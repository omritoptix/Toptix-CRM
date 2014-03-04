namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            CreateTable(
                "dbo.Charge",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChargeDate = c.DateTime(nullable: false),
                        ChargeValue = c.Double(nullable: false),
                        ClientID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        IsAlert = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.OrderID);
            
            AddForeignKey("dbo.OrderItem", "OrderID", "dbo.Order", "ID", cascadeDelete: true);
            CreateIndex("dbo.OrderItem", "OrderID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Charge", new[] { "OrderID" });
            DropIndex("dbo.Charge", new[] { "ClientID" });
            DropIndex("dbo.OrderItem", new[] { "OrderID" });
            DropForeignKey("dbo.Charge", "OrderID", "dbo.Order");
            DropForeignKey("dbo.Charge", "ClientID", "dbo.Client");
            DropForeignKey("dbo.OrderItem", "OrderID", "dbo.Order");
            DropTable("dbo.Charge");
            CreateIndex("dbo.OrderItem", "OrderID");
            AddForeignKey("dbo.OrderItem", "OrderID", "dbo.Order", "ID");
        }
    }
}
