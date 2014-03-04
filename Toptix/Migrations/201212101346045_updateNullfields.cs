namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNullfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "EnumChargeFrequencyID", c => c.Int());
            AlterColumn("dbo.Order", "SupportCalculatedValue", c => c.Double());
            AlterColumn("dbo.Order", "SupportPriceToCharge", c => c.Double());
            AlterColumn("dbo.Order", "EnumCalculationTypeId", c => c.Int());
            AlterColumn("dbo.Order", "SupportDiscountValue", c => c.Double());
            AlterColumn("dbo.Order", "SupportNetPriceToCharge", c => c.Double());
            AlterColumn("dbo.Order", "SupportFirstChargeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "SupportFirstChargeDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Order", "SupportNetPriceToCharge", c => c.Double(nullable: false));
            AlterColumn("dbo.Order", "SupportDiscountValue", c => c.Double(nullable: false));
            AlterColumn("dbo.Order", "EnumCalculationTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Order", "SupportPriceToCharge", c => c.Double(nullable: false));
            AlterColumn("dbo.Order", "SupportCalculatedValue", c => c.Double(nullable: false));
            AlterColumn("dbo.Order", "EnumChargeFrequencyID", c => c.Int(nullable: false));
        }
    }
}
