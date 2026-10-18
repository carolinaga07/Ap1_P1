using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models
{
    public class EntradasDetalle
    {
        [Key]
        public int DetalleId { get; set; }
        public int EntradaId { get; set; }
        public int LibroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="La cantidad debe ser mayor a 0")]
        public double Cantidad { get; set; }

        [ForeignKey("EntradaId")]
        [InverseProperty("EntradaDetalle")]
        public virtual Entradas Entrada { get; set; } = null;

        [ForeignKey("LibroId")]
        [InverseProperty("EntradaDetalle")]
        public virtual Libros Libro { get; set; } = null;
    }
}
