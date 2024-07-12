using System.Security.Claims;
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

        [Authorize(Policy = "--IsWorker")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Servicio> GetAvailiableServices(FixItDBContext context, ClaimsPrincipal claimsPrincipal)
        {
            int id = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var worker = context.Trabajador.Find(id);
            // Get all services that are not assigned to any worker and that are in the same category as the worker and that are in the availability of the worker
            return context
            .Servicio
            .Where(servicio => servicio.Trabajador.Email == "defaultworker@mail.co")
            .Where(servicio => worker.TrabajadorServicios.Any(e => e.Servicio.CategoriaServicio.ID_Servicio == servicio.CategoriaServicio.ID_Servicio));
        }
        [Authorize(Policy = "--IsClient")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Ubicacion> GetClientLocations(FixItDBContext context, ClaimsPrincipal claimsPrincipal)
        {
            int id = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return context.Ubicacion.Where(ubicacion => ubicacion.Cliente.ID == id);
        }
        [Authorize(Policy = "--IsClient")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Servicio> GetClientServices(FixItDBContext context, ClaimsPrincipal claimsPrincipal)
        {
            int id = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return context.Servicio.Where(s => s.Cliente.ID == id);
        }
        [Authorize(Policy = "--IsClient")]
        [UseOffsetPaging]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Calificacion> GetCalificacions(FixItDBContext context, ClaimsPrincipal claimsPrincipal)
        {
            int id = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return context.Calificacion.Where(s => s.Servicio.Cliente.ID == id);
        }


    }
}
