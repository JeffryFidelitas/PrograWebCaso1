using Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caso1.Core.Models
{
    #region Modelo Principal
    public class Ruta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Favor ingrese el codigo de la ruta"), MaxLength(10)]
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Favor ingrese el nombre de la ruta"), MaxLength(100)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Favor ingrese la descripción de la ruta"), MaxLength(100)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [DisplayName("Paradas")]
        // Relación MUCHOS A MUCHOS con Paradas
        public ICollection<RutaParada> RutasParadas { get; set; } = new List<RutaParada>();

        [DisplayName("Horarios")]
        // Relación MUCHOS A MUCHOS con Horarios
        public ICollection<RutaHorario> RutasHorarios { get; set; } = new List<RutaHorario>();

        [Required(ErrorMessage = "Favor seleccione el estado de la ruta")]
        [DisplayName("Estado")]
        public EstadoRuta? Estado { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("UsuarioRegistro")]
        public int UsuarioRegistroId { get; set; }
        public Usuario UsuarioRegistro { get; set; }

        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }

    public class Parada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [DisplayName("Nombre Parada")]
        public string Nombre { get; set; }

        // Relación N:M con Rutas a través de RutaParada
        public ICollection<RutaParada> RutasParadas { get; set; } = new List<RutaParada>();
    }

    public class RutaParada
    {
        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        public int ParadaId { get; set; }
        public Parada Parada { get; set; }

        [Required]
        public int Orden { get; set; }
    }

    public class Horario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        // Relación N:M con Rutas a través de RutaHorario
        public ICollection<RutaHorario> RutasHorarios { get; set; } = new List<RutaHorario>();
    }

    public class RutaHorario
    {
        [Required]
        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        [Required]
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }
    }

    public enum EstadoRuta
    {
        Activo,
        Inactivo
    }
    #endregion

    #region Modelos de Vista

    public class RutaCRUDViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Favor ingrese el codigo de la ruta"), MaxLength(10)]
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Favor ingrese el nombre de la ruta"), MaxLength(100)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Favor ingrese la descripción de la ruta"), MaxLength(100)]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Favor seleccione el estado de la ruta")]
        [DisplayName("Estado")]
        public EstadoRuta? Estado { get; set; }

        [DisplayName("Horarios")]
        [RequiredList(ErrorMessage = "Favor seleccione al menos un horario")]
        public List<int> HorariosSeleccionados { get; set; } = new List<int>();

        [DisplayName("Paradas")]
        [RequiredList(ErrorMessage = "Favor seleccione al menos una parada")]
        public List<int> ParadasSeleccionadas { get; set; } = new List<int>();
    }

    public class RutaListViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public EstadoRuta? Estado { get; set; }
        public List<TimeSpan> Horarios { get; set; } = new List<TimeSpan>();
    }

    public class RutaDetailsViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string FechaRegistro { get; set; }
        public string NombreUsuarioRegistro { get; set; }
        public List<string> Paradas { get; set; } = new List<string>();
        public List<string> Horarios { get; set; } = new List<string>();
    }

    #endregion
}
