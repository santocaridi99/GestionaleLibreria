namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixVenditaDettaglioRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VenditaDettaglios", "LibroId", "dbo.Libroes");
            DropForeignKey("dbo.VenditaDettaglios", "VenditaId", "dbo.Venditas");
            AddForeignKey("dbo.VenditaDettaglios", "LibroId", "dbo.Libroes", "Id");
            AddForeignKey("dbo.VenditaDettaglios", "VenditaId", "dbo.Venditas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VenditaDettaglios", "VenditaId", "dbo.Venditas");
            DropForeignKey("dbo.VenditaDettaglios", "LibroId", "dbo.Libroes");
            AddForeignKey("dbo.VenditaDettaglios", "VenditaId", "dbo.Venditas", "Id");
            AddForeignKey("dbo.VenditaDettaglios", "LibroId", "dbo.Libroes", "Id", cascadeDelete: true);
        }
    }
}
