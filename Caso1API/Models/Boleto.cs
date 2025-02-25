using Caso1;
using System.ComponentModel.DataAnnotations;

namespace Caso1API.Models
{
    public class Boleto
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}
