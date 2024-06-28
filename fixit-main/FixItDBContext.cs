﻿using fixit_main.Models;
using Microsoft.EntityFrameworkCore;

namespace fixit_main
{
    public class FixItDBContext : DbContext
    {
        private readonly string connectionString;
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Ubicacion> Ubicacion { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Certificado> Certificado { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }
        public DbSet<CategoriaServicio> CategoriaServicio { get; set; }
        public DbSet<TrabajadorServicio> TrabajadorServicio { get; set; }
        public DbSet<Calificacion> Calificacion { get; set; }

        public FixItDBContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
