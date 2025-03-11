namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiungiVenditeDettaglio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Venditas", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Venditas", new[] { "ClienteId" });
            AlterColumn("dbo.Venditas", "ClienteId", c => c.Int());
            CreateIndex("dbo.Venditas", "ClienteId");
            AddForeignKey("dbo.Venditas", "ClienteId", "dbo.Clientes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Venditas", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.Venditas", new[] { "ClienteId" });
            AlterColumn("dbo.Venditas", "ClienteId", c => c.Int(nullable: false));
            CreateIndex("dbo.Venditas", "ClienteId");
            AddForeignKey("dbo.Venditas", "ClienteId", "dbo.Clientes", "Id", cascadeDelete: true);
        }
    }
}
