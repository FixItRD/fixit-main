using System.Security.Claims;
using fixit_main.Models;
using fixit_main.Repositories.Templates;
using fixit_main.Services;
using fixit_main.Services.Templates;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace fixit_main.GraphQL
{
    public class RegisterParameters
    {
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
    }
    public class LoginParameters
    {
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RequestServiceParameters
    {
        public DateTime Fecha_Realizacion { get; set; } = DateTime.Now;
        public string Descripcion { get; set; } = "";
        public int ClienteId { get; set; }
        public int Categoria_ServicioId { get; set; }

    }
    public class Availability
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class MutationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    [MutationType]
    public class Mutation
    {
        public async Task<MutationResult> Login(FixItDBContext context, [Service] IServiceHandler serviceHandler, LoginParameters parameters)
        {
            if (parameters.Role == "client")
            {
                var client = context.Cliente.Where(e => e.Email == parameters.Email).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                if (client.Password == serviceHandler._hashingService.HashPasswordWithSalt(parameters.Password, client.Salt))
                    return new MutationResult { Success = true, Message = await serviceHandler._tokenService.GenerateToken(client.ID, client.Nombre, Role.Client) };
                else
                    return new MutationResult { Success = false, Message = "Invalid password" };
            }
            else
            {
                var worker = context.Trabajador.Where(e => e.Email == parameters.Email).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                if (worker.Password == serviceHandler._hashingService.HashPasswordWithSalt(parameters.Password, worker.Salt))
                    return new MutationResult { Success = true, Message = await serviceHandler._tokenService.GenerateToken(worker.ID, worker.Nombre, Role.Worker) };
                else
                    return new MutationResult { Success = false, Message = "Invalid password" };
            }
        }
        public async Task<MutationResult> RegisterUser(FixItDBContext context, [Service] IServiceHandler serviceHandler, RegisterParameters parameters)
        {
            try
            {
                parameters.Password = serviceHandler._hashingService.HashPassword(parameters.Password, out string salt);


                if (parameters.Role == "worker")
                {
                    // Worker exists
                    var t = context.Trabajador.Where(e => e.Email == parameters.Email).FirstOrDefault();
                    if (t != null)
                        return new MutationResult { Success = false, Message = "Worker already exists" };
                    Trabajador trabajador = new() { Nombre = parameters.Name, Email = parameters.Email, Password = parameters.Password, Telefono = parameters.Phone, Salt = salt };
                    await context.Trabajador.AddAsync(trabajador);
                    await context.SaveChangesAsync();
                    return new MutationResult { Success = true, Message = "Worker registered successfully" };
                }
                else
                {

                    var c = context.Cliente.Where(e => e.Email == parameters.Email).FirstOrDefault();
                    if (c != null)
                        return new MutationResult { Success = false, Message = "Client already exists" };
                    Cliente cliente = new() { Nombre = parameters.Name, Email = parameters.Email, Password = parameters.Password, Telefono = parameters.Phone, Salt = salt };

                    await context.Cliente.AddAsync(cliente);
                    await context.SaveChangesAsync();
                    Console.WriteLine(cliente.Salt);
                    return new MutationResult { Success = true, Message = "Client registered successfully" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }

        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> UpdateClientProfile(FixItDBContext context, string name, string phone, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                client.Nombre = name;
                client.Telefono = phone;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> UpdateClientLocation(FixItDBContext context, int locationId, string address, string detail, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var location = context.Ubicacion.Where(e => e.ID_Ubicacion == locationId).FirstOrDefault();
                if (location == null)
                    return new MutationResult { Success = false, Message = "Location not found" };
                location.Direccion = address;
                location.Detalle = detail;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Location updated successfully" };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }

        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> UpdateWorkerProfile(FixItDBContext context, [Service] IServiceHandler serviceHandler, string name, string phone, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                worker.Nombre = name;
                worker.Telefono = phone;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> UpdateAvailabilityDates(FixItDBContext context, [Service] IServiceHandler serviceHandler, List<Availability> availability, ClaimsPrincipal claimsPrincipal)
        {
            string dates = availability.Select(e => e.From.ToString() + " - " + e.To.ToString()).Aggregate((a, b) => a + ", " + b);
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                worker.Disponibilidad = dates;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Availability updated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> AddWorkerService(FixItDBContext context, [Service] IServiceHandler serviceHandler, int categoryId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                var category = context.CategoriaServicio.Where(e => e.ID_Servicio == categoryId).FirstOrDefault();
                if (category == null)
                    return new MutationResult { Success = false, Message = "Category not found" };
                TrabajadorServicio trabajadorServicio = new() { ID_Trabajador = worker.ID, ID_Servicio = category.ID_Servicio };
                await context.TrabajadorServicio.AddAsync(trabajadorServicio);
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service added successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> AddWorkerCertificate(FixItDBContext context, [Service] string area, DateTime fechaOtorgada, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                Certificado certificado = new() { Area = area, Fecha_Otorgada = fechaOtorgada, Validado = false, ID_Trabajador = worker.ID };
                await context.Certificado.AddAsync(certificado);
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Certificate added successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsAdmin")]
        public async Task<MutationResult> ValidateWorkerCertificate(FixItDBContext context, int certificateId)
        {
            try
            {
                var certificate = context.Certificado.Where(e => e.ID_Certificado == certificateId).FirstOrDefault();
                if (certificate == null)
                    return new MutationResult { Success = false, Message = "Certificate not found" };
                certificate.Validado = true;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Certificate validated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }

        [Authorize(Policy = "--IsBoth")]
        public async Task<MutationResult> ChangePassword(FixItDBContext context, [Service] IServiceHandler serviceHandler, string newPassword, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                string? role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

                if (role == "worker")
                {
                    var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                    if (worker == null)
                        return new MutationResult { Success = false, Message = "Worker not found" };
                    worker.Password = serviceHandler._hashingService.HashPassword(newPassword, out string salt);
                    worker.Salt = salt;
                    await context.SaveChangesAsync();
                    return new MutationResult { Success = true, Message = "Password changed successfully" };
                }
                else
                {
                    var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                    if (client == null)
                        return new MutationResult { Success = false, Message = "Client not found" };
                    client.Password = serviceHandler._hashingService.HashPassword(newPassword, out string salt);
                    client.Salt = salt;
                    await context.SaveChangesAsync();
                    return new MutationResult { Success = true, Message = "Password changed successfully" };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        // Create category service mutation, only for admin
        [Authorize(Policy = "--IsAdmin")]
        public async Task<MutationResult> CreateCategoryService(FixItDBContext context, string name)
        {
            try
            {
                var category = context.CategoriaServicio.Where(e => e.Nombre == name).FirstOrDefault();
                if (category != null)
                    return new MutationResult { Success = false, Message = "Category already exists" };
                CategoriaServicio categoriaServicio = new() { Nombre = name };
                await context.CategoriaServicio.AddAsync(categoriaServicio);
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Category created successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> AddLocation(FixItDBContext context, string email, string address, string detail)
        {
            try
            {
                var client = context.Cliente.Where(e => e.Email == email).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                Ubicacion ubicacion = new() { Direccion = address, Detalle = detail, ID_Cliente = client.ID };
                await context.Ubicacion.AddAsync(ubicacion);
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Location added successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> RequestService(FixItDBContext context, RequestServiceParameters parameters, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var category = context.CategoriaServicio.Where(e => e.ID_Servicio == parameters.Categoria_ServicioId).FirstOrDefault();
                if (category == null)
                    return new MutationResult { Success = false, Message = "Category not found" };
                var defaultWorker = context.Trabajador.Where(e => e.Email == "defaultworker@mail.co").FirstOrDefault();
                if (defaultWorker == null)
                    return new MutationResult { Success = false, Message = "Default worker not found" };
                Servicio servicio = new()
                {
                    ClienteId = client.ID,
                    Categoria_ServicioId = category.ID_Servicio,
                    Descripcion = parameters.Descripcion,
                    Fecha_Solicitud = DateTime.Now,
                    Fecha_Realizacion = parameters.Fecha_Realizacion,
                    Fecha_Completado = DateTime.Parse("1753-1-1 00:00:00"),
                    Trabajador = defaultWorker,
                    ID_Calificacion = -1,
                    Factura = new Factura() { Estado_Pago = "pendiente", Fecha_Emision = DateTime.Now, Monto_Total = category.Precio, },
                    Estado = "pendiente",
                };
                await context.Servicio.AddAsync(servicio);
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service requested successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }

        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> AddServiceRating(FixItDBContext context, int serviceId, int rating, string comment, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.ClienteId != client.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                if (service.Estado != "completado")
                    return new MutationResult { Success = false, Message = "Service not completed" };
                if (service.ID_Calificacion != -1)
                    return new MutationResult { Success = false, Message = "Service already rated" };
                Calificacion calificacion = new() { Comentario = comment, Puntuacion = rating };
                await context.Calificacion.AddAsync(calificacion);
                service.ID_Calificacion = calificacion.ID_Calificacion;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service rated successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> PayService(FixItDBContext context, int serviceId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.ClienteId != client.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                if (service.Factura.Estado_Pago == "pagado")
                    return new MutationResult { Success = false, Message = "Service already paid" };
                service.Factura.Estado_Pago = "pagado";
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service paid successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }

        }

        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> TakeService(FixItDBContext context, int serviceId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.Estado != "pendiente")
                    return new MutationResult { Success = false, Message = "Service not available" };
                service.Trabajador = worker;
                service.Estado = "en proceso";
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service taken successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> AbortService(FixItDBContext context, int serviceId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var defaultWorker = context.Trabajador.Where(e => e.Email == "defaultworker@mail.co").FirstOrDefault();
                if (defaultWorker == null)
                    return new MutationResult { Success = false, Message = "Default worker not found" };
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.TrabajadorId != worker.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                service.Trabajador = defaultWorker;
                service.Estado = "pendiente";
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service canceled successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> CancelService(FixItDBContext context, int serviceId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.ClienteId != client.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                service.Estado = "cancelado";
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service canceled successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }
        [Authorize(Policy = "--IsClient")]
        public async Task<MutationResult> ChangeServiceDate(FixItDBContext context, int serviceId, DateTime newDate, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var client = context.Cliente.Where(e => e.ID == id).FirstOrDefault();
                if (client == null)
                    return new MutationResult { Success = false, Message = "Client not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.ClienteId != client.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                service.Fecha_Realizacion = newDate;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service date changed successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }

        }
        [Authorize(Policy = "--IsWorker")]
        public async Task<MutationResult> CompleteService(FixItDBContext context, int serviceId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                _ = int.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id);
                var worker = context.Trabajador.Where(e => e.ID == id).FirstOrDefault();
                if (worker == null)
                    return new MutationResult { Success = false, Message = "Worker not found" };
                var service = context.Servicio.Where(e => e.ID_Servicio == serviceId).FirstOrDefault();
                if (service == null)
                    return new MutationResult { Success = false, Message = "Service not found" };
                if (service.TrabajadorId != worker.ID)
                    return new MutationResult { Success = false, Message = "Service not available" };
                service.Estado = "completado";
                service.Fecha_Completado = DateTime.Now;
                await context.SaveChangesAsync();
                return new MutationResult { Success = true, Message = "Service completed successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MutationResult { Success = false, Message = "An error occurred" };
            }
        }


    }
}
