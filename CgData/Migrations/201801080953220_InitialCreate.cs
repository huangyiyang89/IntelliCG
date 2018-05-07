namespace CgData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DengJi = c.Int(nullable: false),
                        ZhongLei = c.String(),
                        Category1 = c.String(),
                        Category2 = c.String(),
                        NaiJiu = c.Int(nullable: false),
                        NaiJiuMax = c.Int(nullable: false),
                        ShengMing = c.Int(nullable: false),
                        MoLi = c.Int(nullable: false),
                        GongJi = c.Int(nullable: false),
                        FangYu = c.Int(nullable: false),
                        MinJie = c.Int(nullable: false),
                        JingShen = c.Int(nullable: false),
                        HuiFu = c.Int(nullable: false),
                        MeiLi = c.Int(nullable: false),
                        BiSha = c.Int(nullable: false),
                        MingZhong = c.Int(nullable: false),
                        FanJi = c.Int(nullable: false),
                        ShanDuo = c.Int(nullable: false),
                        MoGong = c.Int(nullable: false),
                        KangMo = c.Int(nullable: false),
                        ShiHua = c.Int(nullable: false),
                        Du = c.Int(nullable: false),
                        Zui = c.Int(nullable: false),
                        HunShui = c.Int(nullable: false),
                        YiWang = c.Int(nullable: false),
                        HunLuan = c.Int(nullable: false),
                        Stall_X = c.Int(),
                        Stall_Y = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stalls", t => new { t.Stall_X, t.Stall_Y })
                .Index(t => new { t.Stall_X, t.Stall_Y });
            
            CreateTable(
                "dbo.Stalls",
                c => new
                    {
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        PlayerName = c.String(nullable: false),
                        StallName = c.String(),
                        Description = c.String(),
                        Line = c.String(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.X, t.Y });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", new[] { "Stall_X", "Stall_Y" }, "dbo.Stalls");
            DropIndex("dbo.Items", new[] { "Stall_X", "Stall_Y" });
            DropTable("dbo.Stalls");
            DropTable("dbo.Items");
        }
    }
}
