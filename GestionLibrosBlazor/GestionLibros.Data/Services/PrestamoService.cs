
using Aplicada1.Core;
using GestionLibros.Data.Context;
using GestionLibros.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionLibros.Data.Services;

public class PrestamoService(
    IDbContextFactory<Contexto> contextFactory)
    : IServiceResult<Prestamo, int>
{
    public async Task<Result> Guardar(Prestamo prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            var inserta = await Insertar(prestamo);
            return Result.Success(inserta);
        }
        else
        {
            var modifica = await Modificar(prestamo);
            return Result.Success(modifica);
        }
    }

    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
    }

    private async Task<bool> Insertar(Prestamo prestamo)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        contexto.Prestamos.Add(prestamo);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prestamo prestamo)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        contexto.Prestamos.Update(prestamo);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Prestamo?>> Buscar(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Include(p => p.Estudiante)
            .Include(p => p.Libro)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<Result> Eliminar(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Prestamos
            .Where(p => p.PrestamoId == prestamoId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }

    public async Task<Result<List<Prestamo>>> GetList(
        Expression<Func<Prestamo, bool>> criterio)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Include(p => p.Estudiante)
            .Include(p => p.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> EstudianteTienePrestamo(
        int estudianteId,
        int prestamoId = 0)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .AnyAsync(p =>
                p.EstudianteId == estudianteId &&
                p.FechaDevolucion == null &&
                p.PrestamoId != prestamoId);
    }

    public async Task<bool> LibroEstaPrestado(
        int libroId,
        int prestamoId = 0)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .AnyAsync(p =>
                p.LibroId == libroId &&
                p.FechaDevolucion == null &&
                p.PrestamoId != prestamoId);
    }

    public async Task<bool> RegistrarDevolucion(
        int prestamoId,
        DateOnly fecha)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Where(p => p.PrestamoId == prestamoId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(p => p.FechaDevolucion, fecha)) > 0;
    }
}
