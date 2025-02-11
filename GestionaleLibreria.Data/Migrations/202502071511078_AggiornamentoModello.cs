namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiornamentoModello : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LibroMagazzinoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LibroId = c.Int(nullable: false),
                        MagazzinoId = c.Int(nullable: false),
                        Quantita = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Libroes", t => t.LibroId, cascadeDelete: true)
                .ForeignKey("dbo.Magazzinoes", t => t.MagazzinoId, cascadeDelete: true)
                .Index(t => t.LibroId)
                .Index(t => t.MagazzinoId);
            
            CreateTable(
                "dbo.Magazzinoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Libroes", "ISBN", c => c.String(nullable: false));
            AddColumn("dbo.Libroes", "DurataOre", c => c.Double());
            AddColumn("dbo.Libroes", "Narratore", c => c.String());
            AddColumn("dbo.Libroes", "Formato", c => c.String());
            AddColumn("dbo.Libroes", "DimensioneFile", c => c.Double());
            AddColumn("dbo.Libroes", "Sconto", c => c.Double());
            AddColumn("dbo.Libroes", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Libroes", "Quantita");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Libroes", "Quantita", c => c.Int(nullable: false));
            DropForeignKey("dbo.LibroMagazzinoes", "MagazzinoId", "dbo.Magazzinoes");
            DropForeignKey("dbo.LibroMagazzinoes", "LibroId", "dbo.Libroes");
            DropIndex("dbo.LibroMagazzinoes", new[] { "MagazzinoId" });
            DropIndex("dbo.LibroMagazzinoes", new[] { "LibroId" });
            DropColumn("dbo.Libroes", "Discriminator");
            DropColumn("dbo.Libroes", "Sconto");
            DropColumn("dbo.Libroes", "DimensioneFile");
            DropColumn("dbo.Libroes", "Formato");
            DropColumn("dbo.Libroes", "Narratore");
            DropColumn("dbo.Libroes", "DurataOre");
            DropColumn("dbo.Libroes", "ISBN");
            DropTable("dbo.Magazzinoes");
            DropTable("dbo.LibroMagazzinoes");
        }
    }
}
