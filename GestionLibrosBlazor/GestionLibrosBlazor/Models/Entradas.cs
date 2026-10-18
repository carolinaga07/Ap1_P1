using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }
    public DateOnly Fecha { get; set; }
    public string Concepto { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresa por lo menos un libro.")]
    public int Total { get; set; }

    [InverseProperty("Entrada")]
    public virtual ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();
}
