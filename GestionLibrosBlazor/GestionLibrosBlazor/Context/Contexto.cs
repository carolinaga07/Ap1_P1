using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLibrosBlazor.Context
{
    public class Contexto : DbContext
    {
        public  Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        public DbSet<Libros> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libros>().HasData(
                new List<Libros>()
                {
                    new()
                    {
                        LibroId = 1,
                        Titulo = "Cronicas de Narnia",

                    },
                    new()
                    {
                        LibroId = 2,
                        Titulo = "The perks of being a wallflower",
                    }
                }
            );
            
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
