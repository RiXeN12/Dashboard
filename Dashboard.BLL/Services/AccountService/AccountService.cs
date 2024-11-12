using Dashboard.BLL.Services.MailService;
using Dashboard.BLL.Services.MailService.JwtService;
using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountService(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository, IMailService mailService, IJwtService jwtService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mailService = mailService;
            _configuration = configuration;
            _jwtService = jwtService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse> EmailConfirmAsync(string id, string token)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse("Користувача не знайдено");
            }

            var bytes = WebEncoders.Base64UrlDecode(token);
            var validToken = Encoding.UTF8.GetString(bytes);

            var result = await _userRepository.ConfirmEmailAsync(user, validToken);

            if (!result.Succeeded)
            {
                return ServiceResponse.BadRequestResponse(result.Errors.First().Description);
            }

            return ServiceResponse.OkResponse("Пошта успішно підтверджена");
        }

        public async Task<ServiceResponse> SignInAsync(SignInVM model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email, true);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse($"Користувача з поштою {model.Email} не знайдено");
            }

            var result = await _userRepository.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return ServiceResponse.BadRequestResponse($"Пароль вказано невірно");
            }

            var tokens = await _jwtService.GenerateTokensAsync(user);

            if(!tokens.Success)
            {
                return ServiceResponse.BadRequestResponse("Не вдалося згенерувати токени");
            }

            return ServiceResponse.OkResponse("Успіший вхід", tokens.Payload);
        }

        public async Task<ServiceResponse> SignUpAsync(SignUpVM model)
        {
            if (!await _userRepository.IsUniqueUserNameAsync(model.UserName))
            {
                return ServiceResponse.BadRequestResponse($"{model.UserName} вже викорстовується");
            }

            if (!await _userRepository.IsUniqueEmailAsync(model.Email))
            {
                return ServiceResponse.BadRequestResponse($"{model.Email} вже викорстовується");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return ServiceResponse.BadRequestResponse(result.Errors.First().Description);
            }

            await _userManager.AddToRoleAsync(user, Settings.UserRole);

            await SendConfirmEmailAsync(user);

            var tokens = await _jwtService.GenerateTokensAsync(user);

            if (!tokens.Success)
            {
                return ServiceResponse.BadRequestResponse("Не вдалося згенерувати токени");
            }

            return ServiceResponse.OkResponse($"Користувач {model.Email} успішно зареєстрований", tokens.Payload);
        }

        private async Task SendConfirmEmailAsync(User user)
        {
            string token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
            await _mailService.SendConfirmEmailAsync(user, token);
        }

        public async Task<ServiceResponse> ForgotPasswordAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse("Користувача не знайдено");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                return ServiceResponse.BadRequestResponse("Токен не знайдений");
            }

            var bytes = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(bytes);
            var address = _configuration["Host:Address"];

            string resetUrl = $"{address}/api/account/validateresettoken?email={Email}&token={validToken}";

            string rootPath = _webHostEnvironment.ContentRootPath;
            string templatePath = Path.Combine(rootPath, Settings.HtmlPagesPath, "resetpassword.html");
            string messageText = File.ReadAllText(templatePath);
            messageText = messageText.Replace("resetPasswordUrl", resetUrl);

            await _mailService.SendEmailAsync(Email, "Відновлення паролю", messageText, true);

            return ServiceResponse.OkResponse("Лист для відновлення паролю відправлено на вашу пошту");
        }

        public async Task<ServiceResponse> RestartPasswordAsync(string Email, string Token, string Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return ServiceResponse.BadRequestResponse("Користувача не знайдено");
            }

            var bytes = WebEncoders.Base64UrlDecode(Token);
            var validToken = Encoding.UTF8.GetString(bytes);

            var result = await _userManager.ResetPasswordAsync(user, validToken, Password);

            if (result.Succeeded)
            {
                return ServiceResponse.OkResponse("Пароль було успішно змінено");
            }

            return ServiceResponse.BadRequestResponse("Помилка. Пароль не було змінено");
        }


    }
}
