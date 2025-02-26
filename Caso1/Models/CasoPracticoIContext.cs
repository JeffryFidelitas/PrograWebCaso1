using Microsoft.EntityFrameworkCore;

namespace Caso1.Models
{
    public class CasoPracticoIContext : DbContext

    {
        public CasoPracticoIContext(DbContextOptions<CasoPracticoIContext> options) : base(options) { }

        //Tablas de Entidades
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Rutas> Rutas { get; set; }
        public DbSet<Boletos> Boletos { get; set; }
        public DbSet<Vehiculos> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boletos>()
                .HasOne(b => b.Usuario)
                .WithMany()
                .HasForeignKey(b => b.ID)
                .OnDelete(DeleteBehavior.NoAction); // Evita problemas de cascada

            modelBuilder.Entity<Boletos>()
                .HasOne(b => b.Rutas)
                .WithMany()
                .HasForeignKey(b => b.ID)
                .OnDelete(DeleteBehavior.NoAction); // Evita ciclos de eliminación
        }

    }
}
