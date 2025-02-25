using System.ComponentModel.DataAnnotations;

namespace Caso1
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required, MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, Phone]
        public string Telefono { get; set; }

        [Required]
        public string Contraseña { get; set; } // En la BD se guarda encriptada

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