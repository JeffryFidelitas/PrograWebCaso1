using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Favor ingrese el Nombre de Usuario"), MaxLength(50)]
        [DisplayName("Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage="Favor ingrese el Nombre Completo"), MaxLength(100)]
        [DisplayName("Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Favor ingrese el Correo"), EmailAddress, MaxLength(100)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Favor ingrese el Telefono"), Phone, MaxLength(15)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Favor ingrese la Contraseña")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Favor seleccione un Rol")]
        public RolUsuario? Rol { get; set; }

        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }

    public enum RolUsuario
    {
        Administrador,
        Conductor,
        Usuario
    }
}