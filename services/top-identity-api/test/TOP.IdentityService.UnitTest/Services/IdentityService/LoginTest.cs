using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Services.IdentityService
{
    public class LoginTest
    {
        private readonly Faker _faker = new Faker("pt_BR");
        private readonly IIdentityService _identityService;

        public LoginTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _identityService = scopedServices.GetRequiredService<IIdentityService>();
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentEmail_ReturnsResultWithFail()
        {
            var nonExistingEmail = _faker.Person.Email;

            var result = await _identityService.LoginAsync(
                nonExistingEmail,
                _faker.Internet.Password(prefix: "#RSX"));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task LoginAsync_WhenEmailNotConfirmed_ReturnsResultWithFail()
        {
            var emailNotConfirmed = "test2@email.com";

            var result = await _identityService.LoginAsync(
                emailNotConfirmed,
                _faker.Internet.Password(prefix: "#RSX"));

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User email has not been confirmed");
        }
    }
}
