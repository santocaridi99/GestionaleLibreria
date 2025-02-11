namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Cognome = c.String(nullable: false),
                        Email = c.String(),
                        Telefono = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Libroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titolo = c.String(nullable: false, maxLength: 200),
                        Autore = c.String(nullable: false),
                        Prezzo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantita = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Venditas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LibroId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        DataVendita = c.DateTime(nullable: false),
                        QuantitaVenduta = c.Int(nullable: false),
                        Totale = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Libroes", t => t.LibroId, cascadeDelete: true)
                .Index(t => t.LibroId)
                .Index(t => t.ClienteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Venditas", "LibroId", "dbo.Libroes");
            DropForeignKey("dbo.Venditas", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Venditas", new[] { "ClienteId" });
            DropIndex("dbo.Venditas", new[] { "LibroId" });
            DropTable("dbo.Venditas");
            DropTable("dbo.Libroes");
            DropTable("dbo.Clientes");
        }
    }
}
