using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Services.IdentityService
{
    public class ResetPasswordTest
    {
        private readonly Faker _faker = new Faker("pt_BR");
        private readonly IIdentityService _identityService;

        public ResetPasswordTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _identityService = scopedServices.GetRequiredService<IIdentityService>();
        }

        [Fact]
        public async Task ResetPasswordAsync_WithNonExistentEmail_ReturnsResultWithFail()
        {
            var nonExistingEmail = _faker.Person.Email;

            var result = await _identityService.ResetPasswordAsync(
                nonExistingEmail,
                _faker.Internet.Password(),
                _faker.Lorem.Paragraph(2));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task ResetPasswordAsync_WhenTokenIsInvalid_ReturnsResultWithFail()
        {
            var invalidToken = _faker.Lorem.Paragraph(2);

            var result = await _identityService.ResetPasswordAsync(
                "test1@email.com",
                _faker.Internet.Password(),
                invalidToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("InvalidToken:Invalid token.");
        }

        [Fact]
        public async Task ResetPasswordAsync_WithInvalidPassword_ReturnsResultWithFail()
        {
            var email = "test1@email.com";
            var invalidPassword = _faker.Random.AlphaNumeric(15);

            var result = await _identityService.ResetPasswordAsync(
                email,
                invalidPassword,
                await GetResetPasswordToken(email));

            result.Success.Should().BeFalse();
            result.Message.Split(";").Should().SatisfyRespectively(
                err1 => err1.Should().Be("PasswordRequiresNonAlphanumeric:Passwords must have at least one non alphanumeric character."),
                err2 => err2.Should().Be("PasswordRequiresUpper:Passwords must have at least one uppercase ('A'-'Z')."));
        }

        [Fact]
        public async Task ResetPasswordAsync_WithValidTokenAndValidPassword_ReturnsResultWithSuccessAndPasswordReseted()
        {
            var email = "test1@email.com";
            var newPassword = _faker.Internet.Password(length: 15, prefix: "#RSX12");

            var result = await _identityService.ResetPasswordAsync(
                email,
                newPassword,
                await GetResetPasswordToken(email));

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Password reset successfully");
        }

        private async Task<string> GetResetPasswordToken(string email)
        {
            var result = await _identityService
                .PasswordRecoveryAsync(email);

            return result.Item2;
        }
    }
}
