using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionLibrosBlazor.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        public DbSet<Libros> Libros { get; set; }
        public DbSet <Estudiantes > Estudiantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libros>().HasData(
                new Libros
                {


                    LibroId = 1,
                    Titulo = "Cronicas de Narnia",
                    Autor = "C.S Lewis",
                    AnioPublicacion = 1950,

                },
                new Libros
                {
                    LibroId = 2,
                    Titulo = "The perks of being a wallflower",
                    Autor = "Stephen Chbosky",
                    AnioPublicacion = 1999,
                }

            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
