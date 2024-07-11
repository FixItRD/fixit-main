using fixit_main.Models;
using fixit_main.Models.Templates;
using Microsoft.EntityFrameworkCore;

namespace fixit_main
{
    public class FixItDBContext : DbContext
    {
        public FixItDBContext(DbContextOptions<FixItDBContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Ubicacion> Ubicacion { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Certificado> Certificado { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }
        public DbSet<CategoriaServicio> CategoriaServicio { get; set; }
        public DbSet<TrabajadorServicio> TrabajadorServicio { get; set; }
        public DbSet<Calificacion> Calificacion { get; set; }
    }
}
