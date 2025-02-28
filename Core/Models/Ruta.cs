using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Caso1.Core.Models
{
    public class Ruta
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Codigo { get; set; }  // Se generará automáticamente

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public List<string> Paradas { get; set; } = new List<string>();  // Se almacena como JSON en la BD

        public List<string> Horarios { get; set; } = new List<string>(); // Se almacena como JSON en la BD

        [Required]
        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public int UsuarioRegistroId { get; set; }
    }
}
