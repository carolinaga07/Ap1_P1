using Aplicada1.Core;
using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionLibrosBlazor.Services;

public class EntradasService(
    IDbContextFactory<Contexto> contextFactory
    ) : Aplicada1.Core.IServiceResult<Entradas, int>
{

    public async Task<Result> Guardar(Entradas entradas)
    {
        if(!await Existe(entradas.EntradaId))
        {
            var inserta = await Insertar(entradas);
            return Result.Success(inserta);
        }
        else
        {
            var modifica = await Modificar(entradas);
            return Result.Success(modifica);
        }
    }

    private async Task<bool> Existe( int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .AnyAsync(e => e.EntradaId == entradaId);
    }
    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        await AfectarLibros(entrada.EntradasDetalles.ToArray(), TipoOperacionEntrada.Suma, contexto);
        return await contexto.SaveChangesAsync() > 0;
    }
    private async Task<bool> Modificar(Entradas entradas)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var entradaOriginal = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradas.EntradaId);
        
        if (entradaOriginal == null)
        {
            return false;
        }

        await AfectarLibros(entradaOriginal.EntradasDetalles.ToArray(), TipoOperacionEntrada.Resta, contexto);
        contexto.EntradasDetalles.RemoveRange(entradaOriginal.EntradasDetalles);
        await AfectarLibros(entradas.EntradasDetalles.ToArray(), TipoOperacionEntrada.Suma, contexto);
        
        entradaOriginal.Fecha = entradas.Fecha;
        entradaOriginal.Concepto = entradas.Concepto;
        entradaOriginal.Total = entradas.Total;
        entradaOriginal.EntradasDetalles = entradas.EntradasDetalles;

        contexto.Entradas.Update(entradaOriginal);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarLibros(EntradasDetalle [] detalle, TipoOperacionEntrada tipoOperacion, Contexto contexto)
    {
        foreach (var item in detalle)
        {
            var libro = await contexto.Libros.SingleAsync(l => l.LibroId == item.LibroId);
            if(tipoOperacion == TipoOperacionEntrada.Suma)
            {
                libro.Cantidad += item.Cantidad;
            }
            else
            {
                libro.Cantidad -= item.Cantidad;
            }
        }
    }
    public async Task<Result<Entradas?>> Buscar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .ThenInclude(d => d.Libro)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<Result> Eliminar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var entrada = await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entrada == null) 
        {
            return Result.Success(false);
        }

        await AfectarLibros(entrada.EntradasDetalles.ToArray(), TipoOperacionEntrada.Resta, contexto);
        contexto.EntradasDetalles.RemoveRange(entrada.EntradasDetalles);
        contexto.Entradas.Remove(entrada);
        var eliminado = await contexto.SaveChangesAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradasDetalles)
            .ThenInclude(d => d.Libro)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public enum TipoOperacionEntrada
    {
        Suma = 1,
        Resta = 2
    }

}
