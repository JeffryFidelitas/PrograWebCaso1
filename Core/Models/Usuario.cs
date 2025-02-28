using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required, StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, Phone]
        public string Telefono { get; set; }

        [Required]
        public string Contraseña { get; set; } // Se almacenará con hash en la BD

        [Required]
        public RolUsuario Rol { get; set; }
    }

    public enum RolUsuario
    {
        Administrador,
        Conductor,
        Usuario
    }
}
