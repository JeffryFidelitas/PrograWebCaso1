using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Ruta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Codigo { get; set; } // Generado en la vista con Razor

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<string> Paradas { get; set; } = new();

        public List<string> Horarios { get; set; } = new();

        [Required]
        public EstadoRuta Estado { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public int UsuarioRegistroId { get; set; } // Clave foránea a Usuario
        public Usuario UsuarioRegistro { get; set; }

        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }

    public enum EstadoRuta
    {
        Activo,
        Inactivo
    }
}
