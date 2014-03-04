namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enumactiontype : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "EnumActionTypeID", "dbo.EnumActionType");
            DropIndex("dbo.Order", new[] { "EnumActionTypeID" });
            DropColumn("dbo.Order", "EnumActionTypeID");
            DropTable("dbo.EnumActionType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EnumActionType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Order", "EnumActionTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Order", "EnumActionTypeID");
            AddForeignKey("dbo.Order", "EnumActionTypeID", "dbo.EnumActionType", "ID");
        }
    }
}
