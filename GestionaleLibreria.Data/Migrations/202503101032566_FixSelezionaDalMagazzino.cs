namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSelezionaDalMagazzino : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LibroMagazzinoes", "IsSelected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LibroMagazzinoes", "IsSelected", c => c.Boolean(nullable: false));
        }
    }
}
