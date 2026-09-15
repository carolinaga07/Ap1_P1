using System.ComponentModel.DataAnnotations;

namespace GestionLibrosBlazor.Models
{
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }

        public string Nombre { get; set; }
        public int Ranking { get; set; }
    }
}
