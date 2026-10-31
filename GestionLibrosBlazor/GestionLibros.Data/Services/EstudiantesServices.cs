using GestionLibros.Data.Context;
using GestionLibros.Data.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;

namespace GestionLibros.Data.Services;

public class EstudiantesService(
    IDbContextFactory<Contexto> contextFactory
    ) : IServiceResult<Estudiante, int>
{
    public async Task<Result> Guardar(Estudiante estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            var insertar = await Insertar(estudiante);
            return Result.Success(insertar);
        }
        else
        {
            var modificar = await Modificar(estudiante);
            return Result.Success(modificar);
        }
    }

    private async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == estudianteId);
    }

    private async Task<bool> Insertar(Estudiante estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Estudiantes.Add(estudiante);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Estudiante estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Estudiantes.Update(estudiante);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Estudiante?>> Buscar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
    }

    public async Task<Result> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Estudiantes
            .Where(e => e.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }

    public async Task<Result<List<Estudiante>>> GetList(
        Expression<Func<Estudiante, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Result<bool>> ExisteNombre(
        string nombre,
        int estudianteId = 0)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Estudiantes
            .AnyAsync(e =>
                e.Nombres == nombre &&
                e.EstudianteId != estudianteId);
    }
}
