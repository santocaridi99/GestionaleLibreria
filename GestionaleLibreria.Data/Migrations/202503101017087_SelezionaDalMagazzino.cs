namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SelezionaDalMagazzino : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LibroMagazzinoes", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LibroMagazzinoes", "IsSelected");
        }
    }
}
