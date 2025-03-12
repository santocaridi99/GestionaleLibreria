namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificaVenditaDettaglio : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VenditaDettaglios", "PrezzoUnitario");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VenditaDettaglios", "PrezzoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
