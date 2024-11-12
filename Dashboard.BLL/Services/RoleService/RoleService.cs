  using AutoMapper;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.RoleRepository;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            var models = _mapper.Map<List<RoleVM>>(roles);

            return ServiceResponse.OkResponse("Ролі отримано", models);
        }

        private async Task<ServiceResponse> GetAsync(Func<string, Task<Role?>> func, string value)
        {
            var role = await func(value);

            if (role == null)
            {
                return ServiceResponse.BadRequestResponse("Не вдалося отримати роль");
            }

            var model = _mapper.Map<RoleVM>(role);

            return ServiceResponse.OkResponse("Роль отримано", model);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            return await GetAsync(_roleRepository.GetByIdAsync, id);
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            return await GetAsync(_roleRepository.GetByNameAsync, name);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if(role == null)
            {
                return ServiceResponse.BadRequestResponse($"Роль з id {id} не знайдено");
            }

            var result = await _roleRepository.DeleteAsync(role);

            return ServiceResponse.ByIdentityResult(result, "Роль успішно видалена");
        }

        public async Task<ServiceResponse> CreteAsync(RoleVM model)
        {
            if(!await _roleRepository.IsUniqueNameAsync(model.Name))
            {
                return ServiceResponse.BadRequestResponse($"Роль з іменем {model.Name} вже існує");
            }

            var role = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                NormalizedName = model.Name.ToUpper()
            };

            var result = await _roleRepository.CreateAsync(role);

            return ServiceResponse.ByIdentityResult(result, "Роль успішно створена");
        }

        public async Task<ServiceResponse> UpdateAsync(RoleVM model)
        {
            if (!await _roleRepository.IsUniqueNameAsync(model.Name))
            {
                return ServiceResponse.BadRequestResponse($"Роль з іменем {model.Name} вже існує");
            }

            var role = await _roleRepository.GetByIdAsync(model.Id);

            if(role == null)
            {
                return ServiceResponse.BadRequestResponse($"Роль з id {model.Id} не знайдено");
            }

            role = _mapper.Map(model, role);

            var result = await _roleRepository.UpdateAsync(role);

            return ServiceResponse.ByIdentityResult(result, "Роль успішно оновлена");
        }

        public async Task<ServiceResponse> AddUserToRole(string userId, string roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse($"User with id {userId} not found");
            }

            if (role == null)
            {
                return ServiceResponse.BadRequestResponse($"Role with id {roleId} not found");
            }

            var result = await _userRepository.AddToRoleAsync(user, role.Name);
            return ServiceResponse.ByIdentityResult(result, "User added to role successfully");
        }

        public async Task<ServiceResponse> RemoveUserFromRole(string userId, string roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = await _roleRepository.GetByIdAsync(roleId);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse($"User with id {userId} not found");
            }

            if (role == null)
            {
                return ServiceResponse.BadRequestResponse($"Role with id {roleId} not found");
            }

            var result = await _userRepository.RemoveFromRoleAsync(user, role.Name);
            return ServiceResponse.ByIdentityResult(result, "User removed from role successfully");
        }


    }
}
