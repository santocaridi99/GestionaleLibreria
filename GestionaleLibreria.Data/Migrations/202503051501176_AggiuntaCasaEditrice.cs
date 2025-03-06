namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AggiuntaCasaEditrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Libroes", "CasaEditrice", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Libroes", "CasaEditrice");
        }
    }
}
