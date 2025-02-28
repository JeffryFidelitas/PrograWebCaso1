using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Boleto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int RutaId { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required, StringLength(10)]
        public string Horario { get; set; }

        [Required]
        public int VehiculoId { get; set; }
    }
}
