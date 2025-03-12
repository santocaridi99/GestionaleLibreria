namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifciaModelloLibroSconto : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Libroes", "Sconto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Libroes", "Sconto", c => c.Double());
        }
    }
}
