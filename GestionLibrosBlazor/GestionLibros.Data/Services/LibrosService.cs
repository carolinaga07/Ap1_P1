
using GestionLibros.Data.Context;
using GestionLibros.Data.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;

namespace GestionLibros.Data.Services;

public class LibrosService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Libro, int>
{
    public async Task<Result> Guardar(Libro libro)
    {
        if (!await Existe(libro.LibroId))
        {
            var insertar = await Insertar(libro);
            return Result.Success(insertar);
        }
        else
        {
            var modificar = await Modificar(libro);
            return Result.Success(modificar);
        }
    }

    private async Task<bool> Existe(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Libros
            .AnyAsync(l => l.LibroId == libroId);
    }

    private async Task<bool> Insertar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Libros.Add(libro);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Libros.Update(libro);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Libro?>> Buscar(int libroId)
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

    public async Task<Result<List<Libro>>> GetList(
        Expression<Func<Libro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Libros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result<bool>> ExisteTitulo(
        string titulo,
        int libroId = 0)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Libros
            .AnyAsync(l =>
                l.Titulo == titulo &&
                l.LibroId != libroId);
    }
}

