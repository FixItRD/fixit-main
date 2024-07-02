using fixit_main.Models;
using fixit_main.Models.Templates;

namespace fixit_main.Repositories.Templates
{
    public interface IUserRepository
    {
        public Task<CrudOperationResponse> CreateUserAsync<T>(T user) where T : class, IUser;
        public Task<CrudOperationResponse> ReadUserAsync<T>(int id) where T : class, IUser;
        public Task<CrudOperationResponse> ReadUserAsync<T>(string email) where T : class, IUser;
        public Task<CrudOperationResponse> UpdateUserAsync<T>(T user) where T : class, IUser;

    }
}
