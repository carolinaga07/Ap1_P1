using GestionLibros.Data.Models;
using GestionLibros.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionLibros.Api.Controllers;

public class PrestamoRequest
{
    public int PrestamoId { get; set; }
    public int EstudianteId { get; set; }
    public int LibroId { get; set; }
    public DateOnly FechaPrestamo { get; set; }
    public DateOnly? FechaDevolucion { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class PrestamosController(
    PrestamoService prestamoService, 
    EstudiantesService estudiantesService,
    LibrosService librosService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await prestamoService.GetList(p => true);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await prestamoService.Buscar(id);
        if(resultado.Value is null)
        {
            return NotFound("El prestamo no existe");
        }
        return Ok("resultado");
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] PrestamoRequest prestamoRequest)
    {
        if(prestamoRequest.EstudianteId <= 0 || prestamoRequest.LibroId <= 0)
        {
            return BadRequest("Debe seleccionar un estudiante y un libro");
        }

        var estudiante = await estudiantesService.Buscar(prestamoRequest.EstudianteId);
        if(estudiante.Value is null)
        {
            return BadRequest("El estudiante no existe");
        }

        var libro = await librosService.Buscar(prestamoRequest.LibroId);
        if(libro.Value is null)
        {
            return BadRequest("El libro no existe");
        }

        if(await prestamoService.EstudianteTienePrestamo(prestamoRequest.EstudianteId))
        {
            return BadRequest("El estudiante ya tiene un prestamo activo");
        }

        if(await prestamoService.LibroEstaPrestado(prestamoRequest.LibroId))
        {
            return BadRequest("El libro ya esta prestado");
        }

        var prestamo = new Prestamo
        {
            EstudianteId = prestamoRequest.EstudianteId,
            LibroId = prestamoRequest.LibroId,
            FechaPrestamo = prestamoRequest.FechaPrestamo,
            FechaDevolucion = prestamoRequest.FechaDevolucion
        };

        var resultado = await prestamoService.Guardar(prestamo);
        if (!resultado.IsSuccess)
        {
            return BadRequest("No se pudo registrar el prestamo");
        }
        return Ok(resultado);
    }

    [HttpPut("{int:int}")]
    public async Task<IActionResult> Modificar(int id, [FromBody] PrestamoRequest prestamoRequest)
    {
        if (id != prestamoRequest.PrestamoId)
            return BadRequest("Los ID no coinciden.");

        var existente = await prestamoService.Buscar(id);
        if (existente.Value is null)
            return NotFound("El préstamo no existe.");

        var estudiante = await estudiantesService.Buscar(prestamoRequest.EstudianteId);
        if (estudiante.Value is null)
            return BadRequest("El estudiante no existe.");

        var libro = await librosService.Buscar(prestamoRequest.LibroId);
        if (libro.Value is null)
            return BadRequest("El libro no existe.");

        if (await prestamoService.EstudianteTienePrestamo(prestamoRequest.EstudianteId, id))
            return BadRequest("El estudiante ya tiene otro préstamo activo.");

        if (await prestamoService.LibroEstaPrestado(prestamoRequest.LibroId, id))
            return BadRequest("El libro ya tiene otro préstamo activo.");

        var prestamo = new Prestamo
        {
            PrestamoId = id,
            EstudianteId = prestamoRequest.EstudianteId,
            LibroId = prestamoRequest.LibroId,
            FechaPrestamo = prestamoRequest.FechaPrestamo,
            FechaDevolucion = prestamoRequest.FechaDevolucion
        };

        var resultado = await prestamoService.Guardar(prestamo);

        if (!resultado.IsSuccess)
            return BadRequest("No se pudo modificar el préstamo.");

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existente = await prestamoService.Buscar(id);
        if(existente.Value is null)
        {
            return NotFound("El prestamo no existe");
        }

        var resultado = await prestamoService.Eliminar(id);
        if (!resultado.IsSuccess)
        {
            return BadRequest("No se pudo eliminar el prestamo");
        }
        return Ok(resultado);
    }

    [HttpPut("{id:int}/devolucion")]
    public async Task<IActionResult> RegistrarDevolucion(int id , [FromBody] DateOnly fecha)
    {
        var existente = await prestamoService.Buscar(id);
        if(existente.Value is null)
        {
            return NotFound("El prestamo no existe");
        }

        if(fecha < existente.Value.FechaPrestamo)
        {
            return BadRequest("la devolucion no puede ser anterior al prestamo");
        }

        var resultado = await prestamoService.RegistrarDevolucion(id, fecha);

        if (!resultado)
        {
            return BadRequest("No sse pudo registrar la devolucion");
        }

        return Ok(new { mensaje = "Devolucion registrada correctamente" });
    }

}
