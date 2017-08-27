namespace ListHell.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.flag",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ads_adid = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ad", t => t.ads_adid)
                .Index(t => t.ads_adid);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.flag", "ads_adid", "dbo.ad");
            DropIndex("dbo.flag", new[] { "ads_adid" });
            DropTable("dbo.flag");
        }
    }
}
