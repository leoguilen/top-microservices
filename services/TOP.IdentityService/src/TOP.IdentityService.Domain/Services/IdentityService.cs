using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Constants;
using TOP.IdentityService.Domain.Helpers;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.Domain.Models;

namespace TOP.IdentityService.Domain.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtConfiguration _jwtConfig;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtConfiguration jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig;
        }

        public async Task<AuthenticationResult> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            var confirmEmailResult = await _userManager
                .ConfirmEmailAsync(user, token);

            if (confirmEmailResult.Succeeded is false)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = IdentityErrorsToMessage(confirmEmailResult.Errors)
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Message = "Email confirmed successfully"
            };
        }

        public async Task<(AuthenticationResult, string)> PasswordRecoveryAsync(string email)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is null)
            {
                return (new AuthenticationResult
                {
                    Success = false,
                    Message = "Email not found",
                }, string.Empty);
            }

            var resetPwdResult = await _userManager
                .GeneratePasswordResetTokenAsync(user);

            return (new AuthenticationResult
            {
                Success = true,
                Message = "Token generated successfully"
            }, resetPwdResult);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var hasConfirmedEmail = await _userManager
                .IsEmailConfirmedAsync(user);

            if (hasConfirmedEmail is false)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "User email has not been confirmed"
                };
            }

            var loginResult = await _signInManager.PasswordSignInAsync(
                user, password, isPersistent: true, lockoutOnFailure: true);

            if (loginResult.IsLockedOut)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "User is blocked"
                };
            }

            if (!loginResult.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "Login failure: invalid password",
                };
            }

            var authResult = await TokenHelper.GenerateAuthResultForUserAsync(user, _jwtConfig, _userManager);
            authResult.Message = "Successful login";

            return authResult;
        }

        public async Task<(AuthenticationResult, ApplicationUser, string)> RegisterAsync(string username, string email, string password)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is not null)
            {
                return (new AuthenticationResult
                {
                    Success = false,
                    Message = "Email already exists"
                }, null, string.Empty);
            }

            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
                Email = email
            };

            var createUserResult = await _userManager
                .CreateAsync(newUser, password);

            if (createUserResult.Succeeded is false)
            {
                return (new AuthenticationResult
                {
                    Success = false,
                    Message = IdentityErrorsToMessage(createUserResult.Errors)
                }, null, string.Empty);
            }

            await _userManager.AddToRoleAsync(newUser, Roles.DEFAULT);

            var confirmEmailToken = await _userManager
                .GenerateEmailConfirmationTokenAsync(newUser);

            return (new AuthenticationResult
            {
                Success = true,
                Message = "Registered successfully"
            }, newUser, confirmEmailToken);
        }

        public async Task<AuthenticationResult> ResetPasswordAsync(string email, string password, string token)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user is null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var resetPwdResult = await _userManager
                .ResetPasswordAsync(user, token, password);

            if (resetPwdResult.Succeeded is false)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Message = IdentityErrorsToMessage(resetPwdResult.Errors)
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }

        private static string IdentityErrorsToMessage(IEnumerable<IdentityError> identityErrors)
        {
            var errors = identityErrors.Select(x => 
                string.Join(":", x.Code, x.Description));
            
            return string.Join(";", errors);
        }
    }
}
