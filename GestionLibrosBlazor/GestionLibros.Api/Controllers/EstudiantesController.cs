using GestionLibrosBlazor.Context;
using GestionLibrosBlazor.Models;
using GestionLibrosBlazor.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionLibros.Api.Controllers;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class EstudiantesController(
    EstudiantesService estudiantesService
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetList()
    {
        var resultado = await estudiantesService
             .GetList(e => true);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Buscar(int id)
    {
        var resultado = await estudiantesService.Buscar(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] Estudiantes estudiantes)
    {
        estudiantes.EstudianteId = 0;
        var resultado = await estudiantesService.Guardar(estudiantes);
        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Modificar(int id, [FromBody]Estudiantes estudiantes)
    {
        if(id != estudiantes.EstudianteId)
        {
            return BadRequest("El Id del estudiante no coincide.");
        }
        var resultado = await estudiantesService.Buscar(id);
        var existe = await estudiantesService.GetList(e => e.EstudianteId == id);
        var guardado = await estudiantesService.Guardar(estudiantes);
        return Ok(guardado);
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
