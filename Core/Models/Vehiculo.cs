using System;
using System.ComponentModel.DataAnnotations;

namespace Caso1.Core.Models
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Placa { get; set; }

        [Required, StringLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int CapacidadPasajeros { get; set; }

        [Required]
        public EstadoVehiculo Estado { get; set; } = EstadoVehiculo.Bueno;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioRegistroId { get; set; }
    }

    public enum EstadoVehiculo
    {
        Bueno,
        Regular,
        NecesitaMantenimiento
    }
}
