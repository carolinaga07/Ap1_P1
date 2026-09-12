using System.ComponentModel.DataAnnotations;

namespace GestionLibrosBlazor.Models
{
    public class Libros
    {
        [Key]

        public int LibroId { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string? Titulo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]

        public string? Autor { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]

        public int AnioPublicacion { get; set;}
        [Required(ErrorMessage = "Este campo es requerido")]




    }
}
