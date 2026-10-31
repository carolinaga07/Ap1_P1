using GestionLibros.Data.Models;
using GestionLibros.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController(
    EstudiantesService estudiantesService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetList()
    {
        var resultado = await estudiantesService.GetList(e => true);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Buscar(int id)
    {
        var resultado = await estudiantesService.Buscar(id);
        if(resultado.Value is null)
        {
            return NotFound("Estudiante no encontrado");
        }
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] Estudiante estudiantes)
    {
        estudiantes.EstudianteId = 0;
        var resultado = await estudiantesService.Guardar(estudiantes);
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Modificar(int id, [FromBody]Estudiante estudiantes)
    {
        if(id != estudiantes.EstudianteId)
        {
            return BadRequest("El Id del estudiante no coincide.");
        }

        var existente = await estudiantesService.Buscar(id);
        if(existente.Value is null)
        {
            return NotFound("Estudiante no encontrado");
        }
        var resultado = await estudiantesService.Guardar(estudiantes);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await estudiantesService.Eliminar(id);
        return Ok(resultado);
    }

    [HttpGet("existe-nombre")]
    public async Task<IActionResult> ExisteNombre(string nombre, int estudianteId = 0)
    {
        var resultado = await estudiantesService.ExisteNombre(nombre, estudianteId);
        return Ok(resultado);
    }
}
