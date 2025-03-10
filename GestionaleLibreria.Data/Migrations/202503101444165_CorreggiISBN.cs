namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorreggiISBN : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Libroes", "ISBN", c => c.String(nullable: false, maxLength: 13));
            CreateIndex("dbo.Libroes", "ISBN", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Libroes", new[] { "ISBN" });
            AlterColumn("dbo.Libroes", "ISBN", c => c.String(nullable: false));
        }
    }
}
