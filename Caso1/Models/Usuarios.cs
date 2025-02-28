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
        [DisplayName("Correo Electrónico")]
        [Required(ErrorMessage = "El Correo Electr�nico es un valor requerido")]
        public string Correo { get; set; }
        [DisplayName("Teléfono ")]
        [Required(ErrorMessage = "El Tel�fono es un valor requerido")]
        public int Telefono { get; set; }
        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "La contraseña es un valor requerido")]
        public string Contrasena { get; set; }
        [DisplayName("Rol")]
        [Required(ErrorMessage = "El Rol es un valor requerido")]
        public int Rol { get; set; }
        public bool Estado { get; set; }

        public string NombreEstado
        {
            get
            {
                if (Estado == true)
                {
                    return "Activo";
                }
                else if (Estado == false)
                {
                    return "Inactivo";
                }
                else
                {
                    return "Desconocido";
                }
            }
        }

        public string NombreRol
        {
            get
            {
                if (Rol == 1)
                {
                    return "Administrador";
                }
                else if (Rol == 2)
                {
                    return "Conductor";
                }
                else if (Rol == 3)
                {
                    return "Usuario";
                }
                else
                {
                    return "Desconocido";
                }
            }
        }
    }
}