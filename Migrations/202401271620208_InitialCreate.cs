namespace EFvsAdoNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dzials",
                c => new
                    {
                        DzialId = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(),
                    })
                .PrimaryKey(t => t.DzialId);
            
            CreateTable(
                "dbo.Pracowniks",
                c => new
                    {
                        PracownikId = c.Int(nullable: false, identity: true),
                        Imie = c.String(),
                        Nazwisko = c.String(),
                        Stanowisko = c.String(),
                        DzialId = c.Int(),
                    })
                .PrimaryKey(t => t.PracownikId)
                .ForeignKey("dbo.Dzials", t => t.DzialId)
                .Index(t => t.DzialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pracowniks", "DzialId", "dbo.Dzials");
            DropIndex("dbo.Pracowniks", new[] { "DzialId" });
            DropTable("dbo.Pracowniks");
            DropTable("dbo.Dzials");
        }
    }
}
