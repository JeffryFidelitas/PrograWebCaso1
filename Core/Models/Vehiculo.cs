using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Placa { get; set; }

        [Required, MaxLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public EstadoVehiculo Estado { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }

        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }

    public enum EstadoVehiculo
    {
        Bueno,
        Regular,
        NecesitaMantenimiento
    }
}
