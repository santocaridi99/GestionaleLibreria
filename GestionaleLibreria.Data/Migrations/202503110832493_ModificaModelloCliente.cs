namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificaModelloCliente : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "Nome", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clientes", "Cognome", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Clientes", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Telefono", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "Telefono", c => c.String());
            AlterColumn("dbo.Clientes", "Email", c => c.String());
            AlterColumn("dbo.Clientes", "Cognome", c => c.String(nullable: false));
            AlterColumn("dbo.Clientes", "Nome", c => c.String(nullable: false));
        }
    }
}
