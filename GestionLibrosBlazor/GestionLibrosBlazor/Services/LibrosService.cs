using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace GestionLibrosBlazor.Services;

    public class LibrosService(
        IDbContextFactory<Contexto> contextFactory
        ) : Aplicada1.Core.IServiceResult<Libros, int>
    { 
        public async Task<Result> Guardar(Libros libros)
        {

            if (!await Existe(libros.LibroId))
            {
                var insertar = await Insertar(libros);
                return Result.Success(insertar);
            }
            else
            {
                var modificar = await Modificar(libros);
                return Result.Success(modificar);
            }
           
        }

        private async Task<bool> Existe (int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AnyAsync(l => l.LibroId == libroId);
        }

        private async Task<bool> Insertar(Libros libros)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Libros.Add(libros);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Libros libro)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(libro);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task <Result<Libros?>> Buscar(int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
           
            return await contexto.Libros
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LibroId == libroId);
        }

        public async Task<Result> Eliminar(int libroId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();

            var eliminado = await contexto.Libros
            .Where(l => l.LibroId == libroId)
            .ExecuteDeleteAsync() > 0;

            return Result.Success(eliminado);
        }

        public async Task<Result<List<Libros>>> GetList(Expression<Func<Libros, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task <Result<bool>> ExisteTitulo(string titulo, int libroId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Libros
                .AnyAsync(l => l.Titulo == titulo && l.LibroId != libroId);
        }
    }
