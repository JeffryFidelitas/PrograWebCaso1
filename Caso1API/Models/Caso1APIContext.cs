using Microsoft.EntityFrameworkCore;

namespace Caso1API.Models.DbConContext
{
    public class Caso1APIContext : DbContext
    {
        public Caso1APIContext(DbContextOptions<Caso1APIContext> options) : base(options)
        {
        }
        public DbSet<Rutas> Rutas { get; }
        public DbSet<Usuarios> Usuarios { get; }
    }
}
