namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiornaClienteEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "Email", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Clientes", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Clientes", new[] { "Email" });
            AlterColumn("dbo.Clientes", "Email", c => c.String(nullable: false));
        }
    }
}
