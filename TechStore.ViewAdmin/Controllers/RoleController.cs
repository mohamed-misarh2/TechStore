using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public RoleController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string name)
        {
            var role = await _userServices.AddRole(name);
            return Ok(role);

        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _userServices.GetAllRoles();
            return Ok(roles);

        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteAsync(string roleId)
        {
            var roles = await _userServices.DeleteRole(roleId);
            return Ok(roles);

        }
    }
}
