namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifciaModelloLibroscontofix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Libroes", "Sconto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Libroes", "Sconto");
        }
    }
}
