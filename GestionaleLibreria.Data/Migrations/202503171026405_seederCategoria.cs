namespace GestionaleLibreria.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seederCategoria : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO Categorias (Nome) SELECT 'Narrativa' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Narrativa');
            INSERT INTO Categorias (Nome) SELECT 'Saggistica' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Saggistica');
            INSERT INTO Categorias (Nome) SELECT 'Fantascienza' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Fantascienza');
            INSERT INTO Categorias (Nome) SELECT 'Giallo' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Giallo');
            INSERT INTO Categorias (Nome) SELECT 'Storia' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Storia');
            INSERT INTO Categorias (Nome) SELECT 'Tecnologia' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Tecnologia');
            INSERT INTO Categorias (Nome) SELECT 'Scienze' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Scienze');
            INSERT INTO Categorias (Nome) SELECT 'Bambini' WHERE NOT EXISTS (SELECT 1 FROM Categorias WHERE Nome = 'Bambini');
        ");
        }

        public override void Down()
        {
            Sql("DELETE FROM Categorias WHERE Nome IN ('Narrativa', 'Saggistica', 'Fantascienza', 'Giallo', 'Storia', 'Tecnologia', 'Scienze', 'Bambini');");
        }
    }
}
