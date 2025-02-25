using Caso1;
using System.ComponentModel.DataAnnotations;

namespace Caso1API.Models
{
    public class Parada
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Orden { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
    }
}
