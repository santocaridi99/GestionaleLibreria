namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaMetodoPagamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Venditas", "MetodoPagamento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Venditas", "MetodoPagamento");
        }
    }
}
