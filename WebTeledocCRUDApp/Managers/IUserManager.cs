using Microsoft.EntityFrameworkCore;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Managers
{
    public interface IUserManager
    {
        Task<User> CreateUser(CreateUserRequest userdata);
        Task<User> UpdateUserData(int id, UpdateUserRequest userdata);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> LoginToAccount(string login, string password);
        Task<User> GetUserByINN(string INN);
        Task<User> RevokeUser(int id);
        Task<User> RestoreUser(int id);
    }
}
