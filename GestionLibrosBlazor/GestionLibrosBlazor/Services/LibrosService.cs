using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;

namespace GestionLibrosBlazor.Services
{
    public class LibrosService(IDbContextFactory<Contexto> contextFactory) : Aplicada1.Core.IService<Libros, string>
    {
       private async Task<bool> Existe(int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros.AnyAsync(l => l.LibroId == libroId);
        }

        private async Task<bool> Insertar(Libros libro)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Libros.Add(libro);
            return await contexto.SaveChangesAsync() > 0;

        }
    }
}
