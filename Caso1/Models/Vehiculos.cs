namespace Caso1
{
    public class Vehiculos
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int CapacidadPasajeros { get; set; }
        public string Estado { get; set; } // Bueno, Regular, Necesita Mantenimiento
        public DateTime FechaRegistro { get; set; }
        public virtual Usuarios Usuario { get; set; }
    }
}