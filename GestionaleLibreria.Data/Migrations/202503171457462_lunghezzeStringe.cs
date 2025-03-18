namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lunghezzeStringe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Libroes", "Autore", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Libroes", "Narratore", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Libroes", "Narratore", c => c.String());
            AlterColumn("dbo.Libroes", "Autore", c => c.String(nullable: false));
        }
    }
}
