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
        public DbSet<Parada> Paradas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<RutaHorario> RutasHorarios { get; set; }
        public DbSet<RutaParada> RutasParadas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación muchos a muchos entre Ruta y Horario
            modelBuilder.Entity<RutaHorario>()
                .HasKey(rh => new { rh.RutaId, rh.HorarioId });

            modelBuilder.Entity<RutaHorario>()
                .HasOne(rh => rh.Ruta)
                .WithMany(r => r.RutasHorarios)
                .HasForeignKey(rh => rh.RutaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RutaHorario>()
                .HasOne(rh => rh.Horario)
                .WithMany(h => h.RutasHorarios)
                .HasForeignKey(rh => rh.HorarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación muchos a muchos entre Ruta y Parada
            modelBuilder.Entity<RutaParada>()
                .HasKey(rp => new { rp.RutaId, rp.ParadaId });

            modelBuilder.Entity<RutaParada>()
                .HasOne(rp => rp.Ruta)
                .WithMany(r => r.RutasParadas)
                .HasForeignKey(rp => rp.RutaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RutaParada>()
                .HasOne(rp => rp.Parada)
                .WithMany(p => p.RutasParadas)
                .HasForeignKey(rp => rp.ParadaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Se asegura que 'Orden' en RutaParada no sea nulo
            modelBuilder.Entity<RutaParada>()
                .Property(rp => rp.Orden)
                .IsRequired();

            // Configuración de relaciones para Boleto con DeleteBehavior.Restrict
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

            // Configurar Horario para que se almacene como TIME en la BD
            modelBuilder.Entity<Horario>()
                .Property(h => h.Hora)
                .HasColumnType("time");

            // Llamar base.OnModelCreating
            base.OnModelCreating(modelBuilder);
        }
    }
}
