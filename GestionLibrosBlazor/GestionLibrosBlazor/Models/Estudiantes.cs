using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace GestionLibrosBlazor.Models
{
    public class Estudiantes
    {
        [Key]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "La direccion es requerido")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="La fecha de nacimiento es requerida")]
        public DateFormat FechaNacimiento { get; set; }


    }
}
