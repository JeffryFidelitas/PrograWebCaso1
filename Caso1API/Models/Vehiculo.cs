using System.ComponentModel.DataAnnotations;

namespace Caso1
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Placa { get; set; }

        [Required, MaxLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int CapacidadPasajeros { get; set; }

        [Required]
        public EstadoVehiculo Estado { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }
    }

    public enum EstadoVehiculo
    {
        Bueno,
        Regular,
        NecesitaMantenimiento
    }
}