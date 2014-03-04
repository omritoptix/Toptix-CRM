namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversionrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversionRate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CurrencyID = c.Int(nullable: false),
                        ConversionValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Currency", t => t.CurrencyID)
                .Index(t => t.CurrencyID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ConversionRate", new[] { "CurrencyID" });
            DropForeignKey("dbo.ConversionRate", "CurrencyID", "dbo.Currency");
            DropTable("dbo.ConversionRate");
        }
    }
}
