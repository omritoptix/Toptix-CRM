namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeforeClientChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "PayTypeID", "dbo.PayType");
            DropIndex("dbo.Order", new[] { "PayTypeID" });
            AddColumn("dbo.Order", "PayConditionID", c => c.Int());
            AddForeignKey("dbo.Order", "PayConditionID", "dbo.PayCondition", "ID");
            CreateIndex("dbo.Order", "PayConditionID");
            DropColumn("dbo.Order", "PayTypeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "PayTypeID", c => c.Int());
            DropIndex("dbo.Order", new[] { "PayConditionID" });
            DropForeignKey("dbo.Order", "PayConditionID", "dbo.PayCondition");
            DropColumn("dbo.Order", "PayConditionID");
            CreateIndex("dbo.Order", "PayTypeID");
            AddForeignKey("dbo.Order", "PayTypeID", "dbo.PayType", "ID");
        }
    }
}
