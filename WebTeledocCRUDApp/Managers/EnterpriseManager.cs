
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Managers
{
    public class EnterpriseManager : IEnterpriseManager
    {
        private readonly TeledocDBContext _dbContext;

        public EnterpriseManager(TeledocDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        private async Task<bool> CheckDataForOriginality(string inn)
        {
            bool containEnterpriseWithThisINN = false;
            if (_dbContext.Enterprises.ToList().Count > 0)
            {
                containEnterpriseWithThisINN = await _dbContext.Enterprises.FirstOrDefaultAsync(x => x.INN == inn) != null;
            }
            if (containEnterpriseWithThisINN)
                throw new Exception($"Данный ИНН уже занят другим предприятием");
            else
                return true;
        }

        public async Task<Enterprise> CreateEnterprise(int userId, CreateEnterpriseRequest enterpriseData)
        {
            var user = await _dbContext.Users.Include(x => x.Enterprises).FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                if (await CheckDataForOriginality(enterpriseData.INN))
                {
                    var newEnterprise = new Enterprise
                    {
                        Name = enterpriseData.Name,
                        INN = enterpriseData.INN,
                        IsIndividual = enterpriseData.IsIndividual,
                        CreatedOn = DateTime.UtcNow,
                    };
                    newEnterprise.Users.Add(user);
                    _dbContext.Enterprises.Add(newEnterprise);
                    await _dbContext.SaveChangesAsync();
                    return newEnterprise;
                }
                else
                    throw new Exception("Введенные данные не оригинальны");
            }
            else
                throw new Exception($"Пользователь с Id - {userId} не найден");

        }
        public async Task<Enterprise> UpdateEnterpriseData(int id, UpdateEnterpriseRequest enterpriseData)
        {
            if (await CheckDataForOriginality(enterpriseData.INN))
            {
                var enterprise = await _dbContext.Enterprises.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
                if (enterprise != null)
                {
                    enterprise.Name = enterpriseData.Name;
                    enterprise.INN = enterpriseData.INN;
                    enterprise.ModifiedOn = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                    return enterprise;
                }
                else
                    throw new Exception($"Предприятие с Id = {id} не найдено");
            }
            else
                throw new Exception("Введенные данные не оригинальны");
        }
        public async Task<List<Enterprise>> GetAllEnterprises()
        {
            var enterprises = await _dbContext.Enterprises.Include(x => x.Users).ToListAsync();
            enterprises = enterprises.FindAll(x => x.RevokedOn == null);
            enterprises.OrderBy(x => x.Name);
            return enterprises;
        }
        public async Task<Enterprise> GetEnterpriseById(int id)
        {
            Enterprise enterprise = await _dbContext.Enterprises.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
            if (enterprise != null)
                return enterprise;
            else
                throw new Exception($"Предприятие с Id - {id} не найдено");
        }
        public async Task<Enterprise> GetEnterpriseByINN(string inn)
        {
            Enterprise enterprise = await _dbContext.Enterprises.Include(x => x.Users).FirstOrDefaultAsync(x => x.INN == inn);
            if (enterprise != null)
                return enterprise;
            else
                throw new Exception($"Предприятие с ИНН - {inn} не найдено");
        }
        public async Task<Enterprise> RevokeEnterprise(int id)
        {
            Enterprise enterprise = await _dbContext.Enterprises.FirstOrDefaultAsync(x => x.Id == id);
            enterprise.RevokedOn = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return enterprise;
        }
        public async Task<Enterprise> RestoreEnterprise(int id)
        {
            Enterprise enterprise = await _dbContext.Enterprises.FirstOrDefaultAsync(x => x.Id == id);
            enterprise.RevokedOn = null;
            await _dbContext.SaveChangesAsync();
            return enterprise;
        }
        public async Task<Enterprise> AddUser(int enterpriseId, int userId)
        {
            Enterprise enterprise = await _dbContext.Enterprises.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == enterpriseId);
            User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
                if (enterprise != null)
                    if ((!enterprise.IsIndividual)||(enterprise.Users.Count()<1))
                    {
                        enterprise.Users.Add(user);
                        await _dbContext.SaveChangesAsync();
                        return enterprise;
                    }
                    else
                        throw new Exception($"ИП не может иметь более одного учредителя");
                else
                    throw new Exception($"Предприятие с Id - {enterpriseId} не найдено");
            else
                throw new Exception($"Пользователь с Id - {userId} не найден");
        }
    }
}
