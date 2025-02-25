using Caso1;
using Microsoft.EntityFrameworkCore;

namespace Caso1API.Models.DbConContext
{
    public class Caso1Context : DbContext
    {
        public Caso1Context(DbContextOptions<Caso1Context> options) : base(options)
        {
        }

        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Ruta> Rutas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Usuario)
                .WithMany()
                .HasForeignKey(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction); // Evita problemas de cascada

            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Ruta)
                .WithMany()
                .HasForeignKey(b => b.RutaId)
                .OnDelete(DeleteBehavior.NoAction); // Evita ciclos de eliminación

            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Horario)
                .WithMany()
                .HasForeignKey(b => b.HorarioId)
                .OnDelete(DeleteBehavior.Cascade); // Puede quedarse con CASCADE si no genera conflictos
        }
    }
}
