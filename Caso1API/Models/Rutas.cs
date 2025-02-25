using Caso1API.Models;
using System.ComponentModel.DataAnnotations;

namespace Caso1
{
    public class Ruta
    {
        public int Id { get; set; }

        [Required]
        public string Codigo { get; set; } // Se genera automáticamente

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<Parada> Paradas { get; set; } = new List<Parada>();

        public List<Horario> Horarios { get; set; } = new List<Horario>();

        [Required]
        public EstadoRuta Estado { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }
    }

    public enum EstadoRuta
    {
        Activo,
        Inactivo
    }
}