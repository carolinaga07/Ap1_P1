using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models
{
    public class PrestamoLibro
    {
        [Key]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage ="Este campo es requerido")]
        public DateTime FechaPrestamo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage =" Debe seleccionar un estudiante valido")]
        public int EstudianteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = " Debe seleccionar un libro valido")]
        public int LibroId { get; set; }

        [ForeignKey("EstudianteId")]
        [InverseProperty("PrestamoLibro")]
        public virtual Estudiantes Estudiantes { get; set; } = null;

        [ForeignKey("LibroId")]
        [InverseProperty("PrestamoLibro")]
        public virtual Libros Libros { get; set; } = null;

    }
}
