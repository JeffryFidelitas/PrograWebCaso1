using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    public class Boleto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        [Required]
        public Horario Horario { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
    }

    public class BoletoListViewModel
    {
        public int Id { get; set; }
        public Usuario? Usuario { get; set; }
        public Ruta? Ruta { get; set; }
        public Horario? Horario { get; set; }
    }

    public class BoletoDetailsViewModel
    {
        public int Id { get; set; }
        public Usuario? Usuario { get; set; }
        public Ruta? Ruta { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public Horario? Horario { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
