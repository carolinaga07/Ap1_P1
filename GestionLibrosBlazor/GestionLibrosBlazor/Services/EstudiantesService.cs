using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System.Linq.Expressions;

namespace GestionLibrosBlazor.Services
{
    public class EstudiantesService(IDbContextFactory<Contexto> contextFactory): Aplicada1.Core.IService<Estudiantes, int>
    {
        public async Task<bool> Guardar(Estudiantes estudiantes)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            if(estudiantes.EstudianteId == 0)
            {
                contexto.Estudiantes.Add(estudiantes);
            }
            else
            {
                contexto.Update(estudiantes);
            }
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Existe (int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes.AnyAsync(e => e.EstudianteId == estudianteId);
        }

        private async Task<bool> Insertar(Estudiantes estudiantes)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Estudiantes.Add(estudiantes);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task <bool> Modificar(Libros libros)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            contexto.Update(libros);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task <Estudiantes?> Buscar(int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes.AsNoTracking().FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
        }

        public async Task <bool> Eliminar(int estudianteId)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            var estudiante = await contexto.Estudiantes.FindAsync(estudianteId);
            if(estudiante == null)
            {
                return false;
            }
            contexto.Estudiantes.Remove(estudiante);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task <List <Estudiantes>> GetList(Expression<Func<Estudiantes, bool>> criterio)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes.Where(criterio).AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExisteNombre(string nombre, int estudianteId = 0)
        {
            await using var contexto = await contextFactory.CreateDbContextAsync();
            return await contexto.Estudiantes.AnyAsync(e => e.Nombres == nombre && e.EstudianteId != estudianteId);
        }
    }
}
