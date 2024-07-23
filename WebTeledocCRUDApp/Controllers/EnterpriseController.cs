using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Managers;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly IEnterpriseManager _enterpriseManager;

        public EnterpriseController(IEnterpriseManager enterpriseManager)
        {
            _enterpriseManager = enterpriseManager;
        }

        [HttpPost("create/{userId:int}")]
        public async Task<Enterprise> CreateEnterprise([FromRoute]int userId,[FromBody] CreateEnterpriseRequest enterpriseData)
        {
            return await _enterpriseManager.CreateEnterprise(userId, enterpriseData);
        }
        [HttpPut("update/{id:int}")]
        public async Task<Enterprise> UpdateEnterpriseData([FromRoute]int id, [FromBody]  UpdateEnterpriseRequest enterpriseData)
        {
            return await _enterpriseManager.UpdateEnterpriseData(id, enterpriseData);
        }
        [HttpGet("get/all")]
        public async Task<List<Enterprise>> GetAllEnterprises()
        {
            return await _enterpriseManager.GetAllEnterprises();
        }
        [HttpGet("get/one/{id:int}")]
        public async Task<Enterprise> GetEnterpriseById([FromRoute]int id)
        {
            return await _enterpriseManager.GetEnterpriseById(id);
        }
        [HttpGet("get/one/inn/{inn}")]
        public async Task<Enterprise> GetEnterpriseByINN([FromRoute]string inn)
        {
            return await _enterpriseManager.GetEnterpriseByINN(inn);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<Enterprise> RevokeEnterprise([FromRoute] int id)
        {
            return await _enterpriseManager.RevokeEnterprise(id);
        }
        [HttpPut("restore/{id:int}")]
        public async Task<Enterprise> RestoreEnterprise([FromRoute] int id)
        {
            return await _enterpriseManager.RestoreEnterprise(id);
        }
        [HttpPut("addUser/{enterpriseId:int}/{userId:int}")]
        public async Task<Enterprise> AddUser([FromRoute] int enterpriseId, [FromRoute] int userId)
        {
            return await _enterpriseManager.AddUser(enterpriseId, userId);
        }
    }
}
