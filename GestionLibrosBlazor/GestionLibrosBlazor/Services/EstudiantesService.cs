using GestionLibrosBlazor.Context;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;
using GestionLibrosBlazor.Models;

namespace GestionLibrosBlazor.Services;

    public class EstudiantesService(
        IDbContextFactory<Contexto> contextFactory
        ): Aplicada1.Core.IServiceResult<Estudiantes, int>
    {
        public async Task<Result> Guardar(Estudiantes estudiantes)
        {
            
            if(!await Existe(estudiantes.EstudianteId))
            {
                var insertar = await Insertar(estudiantes);
                return Result.Success(insertar);
            }
            else
            {
                var modificar = await Modificar(estudiantes);
                return Result.Success(modificar);
            }
        }

        private async Task<bool> Existe (int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == estudianteId);
        }

        private async Task<bool> Insertar(Estudiantes estudiantes)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Estudiantes.Add(estudiantes);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task <bool> Modificar(Estudiantes estudiantes)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(estudiantes);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task <Result<Estudiantes?>> Buscar(int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
        }

        public async Task <Result> Eliminar(int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            
            var eliminado = await contexto.Estudiantes
            .Where(e => e.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;

             return Result.Success(eliminado);
        }

        public async Task <Result<List <Estudiantes>>> GetList(Expression<Func<Estudiantes, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            
            return await contexto.Estudiantes
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Result<bool>> ExisteNombre(string nombre, int estudianteId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes.AnyAsync(e => e.Nombres == nombre && e.EstudianteId != estudianteId);
        }
    }
