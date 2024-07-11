using fixit_main.Models;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

namespace fixit_main.GraphQL
{
    [QueryType]
    public static class Query
    {
        // [Authorize(Policy = "--IsClient")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static async Task<IQueryable<Cliente>> GetClientes(FixItDBContext context)
        {
            IQueryable<Cliente> clientes = context.Cliente;
            await clientes.ForEachAsync(cliente =>
            {
                cliente.Password = "";
                cliente.Salt = "";
            });

            return clientes;
        }
        // [Authorize(Policy = "--IsWorker")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public async static Task<IQueryable<Trabajador>> GetTrabajadores(FixItDBContext context)
        {
            IQueryable<Trabajador> trabajadores = context.Trabajador;
            await trabajadores.ForEachAsync(trabajador =>
            {
                trabajador.Password = "";
                trabajador.Salt = "";
            });

            return trabajadores;
        }
        // [Authorize(Policy = "--IsBoth")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Servicio> GetServicios(FixItDBContext context) => context.Servicio;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Certificado> GetCertificados(FixItDBContext context) => context.Certificado;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Ubicacion> GetUbicaciones(FixItDBContext context) => context.Ubicacion;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        [Authorize]
        public static IQueryable<CategoriaServicio> GetCategoriasServicios(FixItDBContext context) => context.CategoriaServicio;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<TrabajadorServicio> GetTrabajadoresServicios(FixItDBContext context) => context.TrabajadorServicio;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Calificacion> GetCalificaciones(FixItDBContext context) => context.Calificacion;
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Factura> GetFacturas(FixItDBContext context) => context.Factura;
    }
}
