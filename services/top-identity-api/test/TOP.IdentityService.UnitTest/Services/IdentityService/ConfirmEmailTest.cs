using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Services.IdentityService
{
    public class ConfirmEmailTest
    {
        private readonly Faker _faker = new Faker("pt_BR");
        private readonly IIdentityService _identityService;

        public ConfirmEmailTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _identityService = scopedServices.GetRequiredService<IIdentityService>();
        }

        [Fact]
        public async Task ConfirmEmailAsync_WithNonExistentEmail_ReturnsResultWithFail()
        {
            var nonExistingEmail = _faker.Person.Email;

            var result = await _identityService.ConfirmEmailAsync(
                nonExistingEmail,
                _faker.Lorem.Paragraph(2));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Email not found");
        }

        [Fact]
        public async Task ConfirmEmailAsync_WhenTokenIsInvalid_ReturnsResultWithFail()
        {
            var invalidToken = _faker.Lorem.Paragraph(2);

            var result = await _identityService.ConfirmEmailAsync(
                "test1@email.com",
                invalidToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("InvalidToken:Invalid token.");
        }

        [Fact]
        public async Task ConfirmEmailAsync_WhenEmailExistsAndTokenIsValid_ReturnsResultWithSuccess()
        {
            var confirmationTokenInfo = await GetNewConfirmationToken();

            var result = await _identityService.ConfirmEmailAsync(
                confirmationTokenInfo.Item1,
                confirmationTokenInfo.Item2);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Email confirmed successfully");
        }

        private async Task<(string, string)> GetNewConfirmationToken()
        {
            var email = _faker.Person.Email;
            var result = await _identityService.RegisterAsync(
                _faker.Person.UserName,
                email,
                _faker.Internet.Password(length: 15, prefix: "#RSX12"));

            return (email, result.Item3);
        }
    }
}
