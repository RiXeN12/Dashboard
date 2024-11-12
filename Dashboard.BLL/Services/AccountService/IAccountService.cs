using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponse> SignInAsync(SignInVM model);
        Task<ServiceResponse> SignUpAsync(SignUpVM model);
        Task<ServiceResponse> EmailConfirmAsync(string id, string token);
        Task<ServiceResponse> ForgotPasswordAsync(string Email);
        Task<ServiceResponse> RestartPasswordAsync(string Email, string Token, string Password);
    }
}
