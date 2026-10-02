using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models
{
    public class Libros
    {
        [Key]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es requerido")]
        public int AnioPublicacion { get; set; }

        [InverseProperty("Libros")]
        public virtual PrestamoLibro? Prestamo { get; set; }
    }
}
