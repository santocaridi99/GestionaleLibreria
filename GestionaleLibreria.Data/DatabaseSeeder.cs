using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.Data
{
    public class DatabaseSeeder : DropCreateDatabaseIfModelChanges<LibraryContext>
    {
        protected override void Seed(LibraryContext context)
        {
            if (!context.Categorie.Any())
            {
                var categorie = new List<Categoria>
        {
            new Categoria { Nome = "Narrativa" },
            new Categoria { Nome = "Saggistica" },
            new Categoria { Nome = "Fantascienza" },
            new Categoria { Nome = "Giallo" },
            new Categoria { Nome = "Storia" },
            new Categoria { Nome = "Tecnologia" },
            new Categoria { Nome = "Scienze" },
            new Categoria { Nome = "Bambini" }
        };

                context.Categorie.AddRange(categorie);
                context.SaveChanges();
            }
        }
    }

    }
