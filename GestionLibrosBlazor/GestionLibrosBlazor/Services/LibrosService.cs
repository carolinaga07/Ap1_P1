using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;

namespace GestionLibrosBlazor.Services
{
    public class LibrosService(IDbContextFactory<Contexto> contextFactory) : Aplicada1.Core.IService<Libros, int>
    { 
        public async Task<bool> Guardar(Libros libros)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();

            if (libros.LibroId == 0)
            {
                contexto.Libros.Add(libros);
            }
            else
            {
                contexto.Update(libros);
            }
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task <Libros?> Buscar(int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.LibroId == libroId);

        }

        public async Task<bool> Eliminar(int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            var libro = await contexto.Libros.FindAsync(libroId);
            if (libro == null)
            {
                return false;
            }
            contexto.Libros.Remove(libro);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<Libros>> GetList(Expression<Func<Libros, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros.Where(criterio).AsNoTracking().ToListAsync();
        }
    }
}
