using Microsoft.EntityFrameworkCore;

namespace Caso1.Models
{
    public class CasoPracticoIContext : DbContext

    {
        public CasoPracticoIContext(DbContextOptions<CasoPracticoIContext> options) : base(options) { }

        //Tablas de Entidades
        public DbSet<Usuarios> Usuarios { get; set; }

    }
}
