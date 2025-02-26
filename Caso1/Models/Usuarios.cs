using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Caso1
{
    public class Usuarios
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Nombre Completo")]
        [Required(ErrorMessage = "El Nombre Completo es un valor requerido")]
        public string NombreCompleto { get; set; }
        [DisplayName("Nombre de Usuario")]
        [Required(ErrorMessage = "El Nombre de Usuario es un valor requerido")]
        public string NombreDeUsuario { get; set; }
        [DisplayName("Correo Electr�nico")]
        [Required(ErrorMessage = "El Correo Electr�nico es un valor requerido")]
        public string Correo { get; set; }
        [DisplayName("Tel�fono ")]
        [Required(ErrorMessage = "El Tel�fono es un valor requerido")]
        public string Telefono { get; set; }
        [DisplayName("Contrase�a ")]
        [Required(ErrorMessage = "La contrase�a es un valor requerido")]
        public string Contrasenna { get; set; }
        [DisplayName("Rol")]
        [Required(ErrorMessage = "El Rol es un valor requerido")]
        public string Rol { get; set; }
        public bool Estado { get; set; }
    }
}