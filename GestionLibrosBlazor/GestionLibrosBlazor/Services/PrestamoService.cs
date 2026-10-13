using Aplicada1.Core;
using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionLibrosBlazor.Services;
    public class PrestamoService(
        IDbContextFactory<Contexto> contextFactory)
       : Aplicada1.Core.IServiceResult<PrestamoLibro,int>
    {
         public async Task<bool> Guardar(PrestamoLibro prestamo)
         {
            if (!await Existe(prestamo.PrestamoId))
            {
            return await Insertar(prestamo);
            }
           else
           {
            return await Modificar(prestamo);
           }
         }
        private async Task<bool> Existe(int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
        }

        private async Task<bool> Insertar(PrestamoLibro prestamo)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Prestamos.Add(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(PrestamoLibro prestamo)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(prestamo);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<PrestamoLibro?> Buscar (int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Include(p => p.Estudiantes)
                .Include(p => p.Libros)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        }

        public async Task<Result> Eliminar(int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            
            var eliminado = await contexto.Prestamos
                .Where(p => p.PrestamoId == prestamoId)
                .ExecuteDeleteAsync() > 0;

           return Result.Success(eliminado);
        }

        public async Task<Result<List<PrestamoLibro>>> GetList(Expression<Func<PrestamoLibro, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Include(p => p.Estudiantes)
                .Include(p => p.Libros)
                .Where(criterio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> EstudianteTienePrestamo(int estudianteId, int prestamoId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .AnyAsync(p => p.EstudianteId == estudianteId && p.FechaDevolucion == null && p.PrestamoId!= prestamoId);
        }

        public async Task<bool> LibroEstaPrestado(int libroId, int prestamoId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .AnyAsync(p => p.LibroId == libroId && p.FechaDevolucion == null && p.PrestamoId != prestamoId);
        }
        
        public async Task<bool> RegistrarDevolucion(int prestamoId, DateOnly fecha)
        {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Where(p => p.PrestamoId == prestamoId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.FechaDevolucion, fecha)) > 0;

        }


    }
