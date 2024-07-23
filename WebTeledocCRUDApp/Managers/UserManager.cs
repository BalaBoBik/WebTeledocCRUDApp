using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Managers
{
    public class UserManager : IUserManager
    {
        private readonly TeledocDBContext _dbContext;

        public UserManager(TeledocDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        private async Task<bool> CheckDataForOriginality(string email,string phone, string inn="Не указан")
        {
            bool containUserWithThisEmail = false;
            bool containUserWithThisPhone = false;
            bool containUserWithThisINN = false;
            if (_dbContext.Users.ToList().Count > 0)
            {
                containUserWithThisEmail = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
                containUserWithThisPhone = await _dbContext.Users.FirstOrDefaultAsync(x => x.Phone == phone) != null;
                containUserWithThisINN = await _dbContext.Users.FirstOrDefaultAsync(x => x.INN == inn) != null;
            }
            if (containUserWithThisEmail)
                throw new Exception($"Данная почта уже занята другим пользователем");
            else if (containUserWithThisPhone)
                throw new Exception($"Данный номер телефона уже занят другим пользователем");
            else if (containUserWithThisINN)
                throw new Exception($"Данный ИНН уже занят другим пользователем");
            else
                return true;
        }
        public async Task<User> CreateUser(CreateUserRequest userdata)
        {

            if (await CheckDataForOriginality(userdata.Email, userdata.Phone, userdata.INN))
            {
                var newUser = new User
                {
                    Email = userdata.Email,
                    Phone = userdata.Phone,
                    Password = userdata.Password,
                    FamilyName = userdata.FamilyName,
                    Name = userdata.Name,
                    Patronymic = userdata.Patronymic,
                    INN = userdata.INN,
                    CreatedOn = DateTime.UtcNow,
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                return newUser;
            }
            else
            {
                throw new Exception("Введенные данные не оригинальны");
            }
        }
        public async Task<User> UpdateUserData(int id, UpdateUserRequest userdata)
        {
            if (await CheckDataForOriginality(userdata.Email, userdata.Phone))
            {
                var user = await _dbContext.Users.Include(x=>x.Enterprises).FirstOrDefaultAsync((x => x.Id == id));


                if (user != null)
                {
                    user.Email = userdata.Email;
                    user.Phone = userdata.Phone;
                    user.Password = userdata.Password;
                    user.FamilyName = userdata.FamilyName;
                    user.Name = userdata.Name;
                    user.Patronymic = userdata.Patronymic;
                    user.ModifiedOn = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                    return user;
                }
                else
                    throw new Exception($"Пользователь с Id - {id} не найден");
            }
            else
                throw new Exception("Введенные данные не оригинальны");
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _dbContext.Users.Include(x => x.Enterprises).ToListAsync();
            users = users.FindAll(x => x.RevokedOn == null);
            users.OrderBy(x => x.CreatedOn);
            return users;
        }
        public async Task<User> GetUserById(int id)
        {
            User user = await _dbContext.Users.Include(x => x.Enterprises).FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
                return user;
            else
                throw new Exception($"Пользователь с Id - {id} не найден");
        }
        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _dbContext.Users.Include(x => x.Enterprises).FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
                return user;
            else
                throw new Exception($"Пользователь с почтой - {email} не найден");
        }
        public async Task<User> LoginToAccount(string login, string password)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => (x.Email == login || x.Phone == login) && x.Password == password);

            if (user != null)
                return user;
            else
                throw new Exception("Неверно указан логин или пароль");
        }
        public async Task<User> GetUserByINN(string INN)
        {
            User user = await _dbContext.Users.Include(x => x.Enterprises).FirstOrDefaultAsync(x => x.INN == INN);
            if (user != null)
                return user;
            else
                throw new Exception($"Пользователь с ИНН - {INN} не найден");
        }
        public async Task<User> RevokeUser(int id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.RevokedOn = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> RestoreUser(int id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.RevokedOn = null;
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
