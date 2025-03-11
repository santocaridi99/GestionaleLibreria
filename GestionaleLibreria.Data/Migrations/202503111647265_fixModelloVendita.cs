namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixModelloVendita : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Venditas", "LibroId", "dbo.Libroes");
            DropIndex("dbo.Venditas", new[] { "LibroId" });
            DropColumn("dbo.Venditas", "LibroId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Venditas", "LibroId", c => c.Int(nullable: false));
            CreateIndex("dbo.Venditas", "LibroId");
            AddForeignKey("dbo.Venditas", "LibroId", "dbo.Libroes", "Id", cascadeDelete: true);
        }
    }
}
