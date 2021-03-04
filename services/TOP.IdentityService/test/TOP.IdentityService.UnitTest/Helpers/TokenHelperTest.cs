using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Configurations;
using TOP.IdentityService.Domain.Helpers;
using TOP.IdentityService.Domain.Models;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Helpers
{
    public class TokenHelperTest
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenHelperTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _jwtConfig = scopedServices.GetRequiredService<JwtConfiguration>();
            _userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
        }

        [Fact]
        public async Task GenerateAuthResultForUserAsync_WithValidParameters_ReturnsAuthenticationResultWithAccessToken()
        {
            var user = await _userManager.FindByEmailAsync("test3@email.com");

            var result = await TokenHelper
                .GenerateAuthResultForUserAsync(user, _jwtConfig, _userManager);

            result.Success.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();

            var tokenClaims = GetClaimsByToken(result.Token);

            tokenClaims.First(x => x.Type == "email").Value.Should().Be(user.Email);
            tokenClaims.First(x => x.Type == "sub").Value.Should().Be(user.Id);
        }

        private List<Claim> GetClaimsByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.ToList();
        }
    }
}
