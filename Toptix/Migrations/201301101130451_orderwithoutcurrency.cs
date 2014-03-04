namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderwithoutcurrency : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "CurrencyID", "dbo.Currency");
            DropIndex("dbo.Order", new[] { "CurrencyID" });
            AddColumn("dbo.OrderItem", "CurrencyID", c => c.Int(nullable: false));
            AddForeignKey("dbo.OrderItem", "CurrencyID", "dbo.Currency", "ID");
            CreateIndex("dbo.OrderItem", "CurrencyID");
            DropColumn("dbo.Order", "CurrencyID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "CurrencyID", c => c.Int(nullable: false));
            DropIndex("dbo.OrderItem", new[] { "CurrencyID" });
            DropForeignKey("dbo.OrderItem", "CurrencyID", "dbo.Currency");
            DropColumn("dbo.OrderItem", "CurrencyID");
            CreateIndex("dbo.Order", "CurrencyID");
            AddForeignKey("dbo.Order", "CurrencyID", "dbo.Currency", "ID");
        }
    }
}
