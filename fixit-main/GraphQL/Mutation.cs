using fixit_main.Models;

namespace fixit_main.GraphQL
{
    [MutationType]
    public class Mutation
    {
        public async Task<Cliente> AddCliente(FixItDBContext context, string name)
        {
            Cliente cliente = new Cliente { Nombre = name };

            await context.Cliente.AddAsync(cliente);
            await context.SaveChangesAsync();
            return cliente;
        }
    }
}
