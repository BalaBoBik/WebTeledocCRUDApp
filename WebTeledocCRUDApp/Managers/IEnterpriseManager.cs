using Microsoft.EntityFrameworkCore;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Managers
{
    public interface IEnterpriseManager
    {
        Task<Enterprise> CreateEnterprise(int userId, CreateEnterpriseRequest enterpriseData);
        Task<Enterprise> UpdateEnterpriseData(int id, UpdateEnterpriseRequest enterpriseData);
        Task<List<Enterprise>> GetAllEnterprises();
        Task<Enterprise> GetEnterpriseById(int id);
        Task<Enterprise> GetEnterpriseByINN(string inn);
        Task<Enterprise> RevokeEnterprise(int id);
        Task<Enterprise> RestoreEnterprise(int id);
        Task<Enterprise> AddUser(int enterpriseId, int userId);
    }
}
