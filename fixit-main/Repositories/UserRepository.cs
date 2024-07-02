using fixit_main.Models;
using fixit_main.Models.Templates;
using fixit_main.Repositories.Templates;
using fixit_main.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace fixit_main.Repositories
{
    public class UserRepository : IUserRepository
    {
        FixItDBContext _context;

        public UserRepository(FixItDBContext context)
        {
            _context = context;
        }

        public Task<CrudOperationResponse> CreateUserAsync<T>(T user) where T : class, IUser
        {
            return Task.Run(() =>
            {
                try
                {
                    _context.Set<T>().Add(user);
                    _context.SaveChanges();
                    return new CrudOperationResponse(true, CrudOperation.Create, "User created successfully", new List<object>() { user });
                }
                catch (Exception ex)
                {
                    return new CrudOperationResponse(false, CrudOperation.Create, ex.Message + " | " + ex.InnerException.Message);
                }
            });
        }

        public Task<CrudOperationResponse> ReadUserAsync<T>(int id) where T : class, IUser
        {
            return Task.Run(() =>
            {
                try
                {
                    var user = _context.Set<T>().FindAsync(id);
                    return new CrudOperationResponse(true, CrudOperation.Read, "User found successfully", new List<object>() { user });
                }
                catch (Exception ex)
                {
                    return new CrudOperationResponse(false, CrudOperation.Read, ex.Message + " | " + ex.InnerException.Message);
                }
            });
        }

        public Task<CrudOperationResponse> ReadUserAsync<T>(string email) where T : class, IUser
        {
            return Task.Run(() =>
            {
                try
                {
                    var user = _context.Set<T>().Where(u => u.Email == email).FirstOrDefault();

                    if (user is null)
                    {
                        return new CrudOperationResponse(true, CrudOperation.Read, "User not found");
                    }
                    return new CrudOperationResponse(true, CrudOperation.Read, "User found successfully", new List<object>() { user });
                }
                catch (Exception ex)
                {
                    return new CrudOperationResponse(false, CrudOperation.Read, ex.Message + " | " + ex.InnerException.Message);
                }
            });
        }

        public Task<CrudOperationResponse> UpdateUserAsync<T>(T user) where T : class, IUser
        {
            return Task.Run(() =>
            {
                try
                {
                    T oldUser = _context.Set<T>().Where(u => u.Email == user.Email).FirstOrDefault();

                    if (oldUser is null)
                    {
                        return new CrudOperationResponse(false, CrudOperation.Update, "User not found");
                    }
                    Type userType = typeof(T);
                    PropertyInfo[] props = userType.GetProperties();

                    foreach (var item in props)
                    {
                        if (item.GetValue(user) is null) continue;

                        var value = item.GetValue(user);
                        item.SetValue(oldUser, value);
                    }
                    _context.Entry(oldUser).State = EntityState.Modified;
                    _context.SaveChangesAsync();
                    return new CrudOperationResponse(true, CrudOperation.Update, "User updated successfully", new List<object>() { user });
                }
                catch (Exception ex)
                {
                    return new CrudOperationResponse(false, CrudOperation.Update, ex.Message);
                }
            });
        }
    }
}
