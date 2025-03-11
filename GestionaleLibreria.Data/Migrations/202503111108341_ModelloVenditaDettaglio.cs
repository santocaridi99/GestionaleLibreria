namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelloVenditaDettaglio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VenditaDettaglios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VenditaId = c.Int(nullable: false),
                        LibroId = c.Int(nullable: false),
                        Quantita = c.Int(nullable: false),
                        PrezzoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Libroes", t => t.LibroId, cascadeDelete: true)
                .ForeignKey("dbo.Venditas", t => t.VenditaId)
                .Index(t => t.VenditaId)
                .Index(t => t.LibroId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VenditaDettaglios", "VenditaId", "dbo.Venditas");
            DropForeignKey("dbo.VenditaDettaglios", "LibroId", "dbo.Libroes");
            DropIndex("dbo.VenditaDettaglios", new[] { "LibroId" });
            DropIndex("dbo.VenditaDettaglios", new[] { "VenditaId" });
            DropTable("dbo.VenditaDettaglios");
        }
    }
}
