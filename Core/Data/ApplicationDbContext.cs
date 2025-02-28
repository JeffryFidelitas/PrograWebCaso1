using Microsoft.EntityFrameworkCore;
using Caso1.Core.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            //modelBuilder.Entity<Usuario>()
            //    .Property(u => u.Contraseña)
            //    .HasColumnName("PasswordHash");  // Para evitar almacenar contraseñas en texto plano
            var listValueComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())) : 0,
            c => c != null ? c.ToList() : new List<string>());

            modelBuilder.Entity<Ruta>()
                .Property(r => r.Paradas)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

            modelBuilder.Entity<Ruta>()
                .Property(r => r.Horarios)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
