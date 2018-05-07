namespace CgData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additemforeignkeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", new[] { "Stall_X", "Stall_Y" }, "dbo.Stalls");
            DropIndex("dbo.Items", new[] { "Stall_X", "Stall_Y" });
            RenameColumn(table: "dbo.Items", name: "Stall_X", newName: "StallX");
            RenameColumn(table: "dbo.Items", name: "Stall_Y", newName: "StallY");
            AlterColumn("dbo.Items", "StallX", c => c.Int(nullable: false));
            AlterColumn("dbo.Items", "StallY", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", new[] { "StallX", "StallY" });
            AddForeignKey("dbo.Items", new[] { "StallX", "StallY" }, "dbo.Stalls", new[] { "X", "Y" }, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", new[] { "StallX", "StallY" }, "dbo.Stalls");
            DropIndex("dbo.Items", new[] { "StallX", "StallY" });
            AlterColumn("dbo.Items", "StallY", c => c.Int());
            AlterColumn("dbo.Items", "StallX", c => c.Int());
            RenameColumn(table: "dbo.Items", name: "StallY", newName: "Stall_Y");
            RenameColumn(table: "dbo.Items", name: "StallX", newName: "Stall_X");
            CreateIndex("dbo.Items", new[] { "Stall_X", "Stall_Y" });
            AddForeignKey("dbo.Items", new[] { "Stall_X", "Stall_Y" }, "dbo.Stalls", new[] { "X", "Y" });
        }
    }
}
