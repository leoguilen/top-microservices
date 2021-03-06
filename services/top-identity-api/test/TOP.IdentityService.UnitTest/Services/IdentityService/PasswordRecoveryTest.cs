using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Services.IdentityService
{
    public class PasswordRecoveryTest
    {
        private readonly Faker _faker = new Faker("pt_BR");
        private readonly IIdentityService _identityService;

        public PasswordRecoveryTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _identityService = scopedServices.GetRequiredService<IIdentityService>();
        }

        [Fact]
        public async Task ForgotPasswordRecoveryAsync_WithNonExistentEmail_ReturnsResultWithFail()
        {
            var nonExistingEmail = _faker.Person.Email;

            var result = await _identityService
                .PasswordRecoveryAsync(nonExistingEmail);

            result.Item1.Success.Should().BeFalse();
            result.Item1.Message.Should().Be("Email not found");
        }

        [Fact]
        public async Task ForgotPasswordRecoveryAsync_WithExistentEmail_ReturnsResultWithSuccessAndTokenGenerated()
        {
            var email = "test3@email.com";

            var result = await _identityService
                .PasswordRecoveryAsync(email);

            result.Item1.Success.Should().BeTrue();
            result.Item1.Message.Should().Be("Token generated successfully");
            result.Item2.Should().NotBeNullOrEmpty();
        }
    }
}
