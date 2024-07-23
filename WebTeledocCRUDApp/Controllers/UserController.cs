using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTeledocCRUDApp.Data;
using WebTeledocCRUDApp.Managers;
using WebTeledocCRUDApp.Requests;

namespace WebTeledocCRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<User> CreateUser([FromBody] CreateUserRequest userdata)
        {
            return await _userManager.CreateUser(userdata);
        }
        [HttpPut("update/{id:int}")]
        public async Task<User> UpdateUserData([FromRoute] int id, [FromBody] UpdateUserRequest userdata)
        {
            return await _userManager.UpdateUserData(id, userdata);
        }
        [HttpGet("get/all")]
        public async Task<List<User>> GetAllUsers()
        {
            return await _userManager.GetAllUsers();
        }
        [HttpGet("get/{id:int}")]
        public async Task<User> GetUserById([FromRoute] int id)
        {
            return await _userManager.GetUserById(id);
        }
        [HttpGet("get/email/{email}")]
        public async Task<User> GetUserByEmail([FromRoute] string email)
        {
            return await _userManager.GetUserByEmail(email);
        }
        [HttpGet("login")]
        public async Task<User> LoginToAccount([FromQuery] string login, [FromQuery] string password)
        {
            return await _userManager.LoginToAccount(login, password);
        }
        [HttpGet("get/inn/{INN}")]
        public async Task<User> GetUserByINN([FromRoute] string INN)
        {
            return await _userManager.GetUserByINN(INN);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task<User> RevokeUser([FromRoute] int id)
        {
            return await _userManager.RevokeUser(id);
        }
        [HttpPut("retore/{id:int}")]
        public async Task<User> RestoreUser([FromRoute] int id)
        {
            return await _userManager.RestoreUser(id);
        }
    }
}
