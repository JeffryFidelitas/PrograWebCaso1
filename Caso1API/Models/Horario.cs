using Caso1;
using System.ComponentModel.DataAnnotations;

namespace Caso1API.Models
{
    public class Horario
    {
        public int Id { get; set; }

        [Required]
        public TimeSpan HoraSalida { get; set; }

        [Required]
        public TimeSpan HoraLlegada { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
    }
}
