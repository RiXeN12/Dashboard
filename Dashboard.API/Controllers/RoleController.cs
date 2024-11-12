using Dashboard.BLL.Services;
using Dashboard.BLL.Services.RoleService;
using Dashboard.DAL;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = nameof(Settings.AdminRole) + "," + nameof(Settings.SettingsRole))]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string? id, string? name)
        {
            id = Request.Query[nameof(id)];
            name = Request.Query[nameof(name)];

            if (id == null && name == null)
            {
                var response = await _roleService.GetAllAsync();
                return GetResult(response);
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                var response = await _roleService.GetByIdAsync(id);

                if(response.Success)
                {
                    return GetResult(response);
                }
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                var response = await _roleService.GetByNameAsync(name);

                if (response.Success)
                {
                    return GetResult(response);
                }
            }

            return GetResult(ServiceResponse.BadRequestResponse("Не вдалося отримати роль"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return GetResult(ServiceResponse.BadRequestResponse("id ролі не може бути порожнім"));
            }

            var response = await _roleService.DeleteAsync(id);
            return GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoleVM model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return GetResult(ServiceResponse.BadRequestResponse("Ім'я ролі не може бути порожнім"));
            }

            var response = await _roleService.CreteAsync(model);
            return GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(RoleVM model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return GetResult(ServiceResponse.BadRequestResponse("Ім'я ролі не може бути порожнім"));
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                return GetResult(ServiceResponse.BadRequestResponse("Id ролі не може бути порожнім"));
            }

            var response = await _roleService.UpdateAsync(model);
            return GetResult(response);
        }


        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                return GetResult(ServiceResponse.BadRequestResponse("userId and roleId cannot be empty"));
            }

            var response = await _roleService.AddUserToRole(userId, roleId);
            return GetResult(response);
        }

        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                return GetResult(ServiceResponse.BadRequestResponse("userId and roleId cannot be empty"));
            }

            var response = await _roleService.RemoveUserFromRole(userId, roleId);
            return GetResult(response);
        }
    }
}
