namespace EFvsAdoNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pracowniks", "DzialId", "dbo.Dzials");
            DropIndex("dbo.Pracowniks", new[] { "DzialId" });
            DropColumn("dbo.Pracowniks", "DzialId");
            DropTable("dbo.Dzials");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Dzials",
                c => new
                    {
                        DzialId = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(),
                    })
                .PrimaryKey(t => t.DzialId);
            
            AddColumn("dbo.Pracowniks", "DzialId", c => c.Int());
            CreateIndex("dbo.Pracowniks", "DzialId");
            AddForeignKey("dbo.Pracowniks", "DzialId", "dbo.Dzials", "DzialId");
        }
    }
}
