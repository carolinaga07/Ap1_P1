using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models;

public class EntradasDetalle
{
    [Key]
    public int DetalleId { get; set; }
    public int EntradaId { get; set; }
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage ="La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; }

    [ForeignKey("EntradaId")]
    [InverseProperty("EntradasDetalles")]
    public virtual Entradas Entrada { get; set; } = null;

    [ForeignKey("LibroId")]
    [InverseProperty("EntradasDetalles")]
    public virtual Libros Libro { get; set; } = null;
}
