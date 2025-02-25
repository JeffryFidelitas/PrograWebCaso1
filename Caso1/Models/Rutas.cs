namespace Caso1
{
    public class Rutas
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> Paradas { get; set; }
        public List<DateTime> Horarios { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public virtual Usuarios Usuario { get; set; }

    }
}