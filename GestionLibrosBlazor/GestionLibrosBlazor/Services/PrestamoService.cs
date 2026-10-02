using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionLibrosBlazor.Services
{
    public class PrestamoService(IDbContextFactory<Contexto> contextFactory): Aplicada1.Core.IService<PrestamoLibro,int>
    {
        private async Task<bool> Existe(int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos.AnyAsync(p => p.PrestamoId == prestamoId);
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

        public async Task<PrestamoLibro?> Buscar (int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Include(p => p.Estudiantes)
                .Include(p => p.Libros)
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        }

        public async Task<bool> Eliminar(int prestamoId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos
                .Where(p => p.PrestamoId == prestamoId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<PrestamoLibro>> GetList(Expression<Func<PrestamoLibro, bool>> criterio)
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
                .AnyAsync(p => p.EstudianteId == estudianteId && p.PrestamoId != prestamoId);
        }

        public async Task<bool> LibroEstaPrestado(int libroId, int prestamoId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Prestamos.AnyAsync(p => p.LibroId == libroId && p.PrestamoId != prestamoId);
        }
    }
}
