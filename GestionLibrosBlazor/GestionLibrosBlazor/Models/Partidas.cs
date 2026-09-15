using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLibrosBlazor.Models
{
    public class Partidas
    {
        [Key]
        public int PartidaId { get; set; }

        public DateTime Fecha { get; set; }
        public int Puntuacion { get; set; }

        [ForeignKey("JugadorId")]
        [InverseProperty("Partidas")]
        public virtual Jugadores Jugador { get; set; } = null;
    }
}
