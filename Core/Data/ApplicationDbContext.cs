using Microsoft.EntityFrameworkCore;
using Caso1.Core.Models;

namespace Caso1.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Boleto> Boletos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boleto>()
            .HasOne(b => b.Ruta)
            .WithMany(r => r.Boletos)
            .HasForeignKey(b => b.RutaId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Vehiculo)
                .WithMany(v => v.Boletos)
                .HasForeignKey(b => b.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Usuario)
                .WithMany(u => u.Boletos)
                .HasForeignKey(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
