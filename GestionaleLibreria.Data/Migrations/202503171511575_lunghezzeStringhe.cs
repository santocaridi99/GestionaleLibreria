namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lunghezzeStringhe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Libroes", "CasaEditrice", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Libroes", "CasaEditrice", c => c.String(nullable: false));
        }
    }
}
