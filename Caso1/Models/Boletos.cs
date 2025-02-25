namespace Caso1
{
    public class Boletos
    {
        public int ID { get; set; }
        public DateTime Horario { get; set; }
        public virtual Usuarios Usuario { get; set; }
        public virtual Vehiculos Vehiculos { get; set; }
    }
}