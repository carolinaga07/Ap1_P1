using GestionLibrosBlazor.Models;
using GestionLibrosBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibrosService librosService):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await librosService.GetList(l => true);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await librosService.Buscar(id);
        if(resultado.Value is null)
        {
            return NotFound("El libro no existe");
        }
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] Libros libros)
    {
        libros.LibroId = 0;
        libros.Prestamo = new List<PrestamoLibro>();
        libros.EntradasDetalles = new List<EntradasDetalle>();

        var resultado = await librosService.Guardar(libros);

        if(resultado.IsFailure)
        {
            return BadRequest("No se pudo registrar el libro");
        }
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Modificar(int id, [FromBody] Libros libros)
    {
        if(id != libros.LibroId)
        {
            return BadRequest("Los Id no coinciden");
        }

        var existente = await librosService.Buscar(id);
        if(existente.Value is null)
        {
            return NotFound("El libro no existe");
        }

        libros.Prestamo = new List<PrestamoLibro>();
        libros.EntradasDetalles = new List<EntradasDetalle>();

        var resultado = await librosService.Guardar(libros);

        if (resultado.IsFailure)
        {
            return BadRequest("No se pudo modificar el libro");
        }

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existente = await librosService.Buscar(id);
        if(existente.Value is null)
        {
            return NotFound("El libro no existe");
        }

        var resultado = await librosService.Eliminar(id);

        if (resultado.IsFailure)
        {
            return BadRequest("No se pudo eliminar el libro");
        }

        return Ok(resultado);
    }

    [HttpGet("ExisteTitulo")]
    public async Task<IActionResult> ExisteTitulo(string titulo, int libroId = 0)
    {
        var resultado = await librosService.ExisteTitulo(titulo, libroId);
        return Ok(resultado);
    }
}
