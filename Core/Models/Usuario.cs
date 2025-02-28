using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required, MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Correo { get; set; }

        [Required, Phone, MaxLength(15)]
        public string Telefono { get; set; }

        [Required]
        public string Contraseña { get; set; } // ⚠️ Se recomienda usar Hashing

        [Required]
        public RolUsuario Rol { get; set; } // Enum para definir el rol

        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }

    public enum RolUsuario
    {
        Administrador,
        Conductor,
        Usuario
    }
}