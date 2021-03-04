using System.Threading.Tasks;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Domain.Interfaces.Domain.Services
{
    public interface IIdentityService
    {
        Task<(AuthenticationResult, ApplicationUser, string)> RegisterAsync(string username, string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<(AuthenticationResult, string)> PasswordRecoveryAsync(string email);
        Task<AuthenticationResult> ConfirmEmailAsync(string email, string token);
        Task<AuthenticationResult> ResetPasswordAsync(string email, string password, string token);
    }
}
