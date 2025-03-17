namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiungiCategorie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Libroes", "CategoriaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Libroes", "CategoriaId");
            AddForeignKey("dbo.Libroes", "CategoriaId", "dbo.Categorias", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Libroes", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.Libroes", new[] { "CategoriaId" });
            DropColumn("dbo.Libroes", "CategoriaId");
            DropTable("dbo.Categorias");
        }
    }
}
