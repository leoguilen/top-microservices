using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TOP.IdentityService.Domain.Interfaces.Domain.Services;
using TOP.IdentityService.UnitTest.ConfigureServices;
using Xunit;

namespace TOP.IdentityService.UnitTest.Services.IdentityService
{
    public class RegisterTest
    {
        private readonly Faker _faker = new Faker("pt_BR");
        private readonly IIdentityService _identityService;

        public RegisterTest()
        {
            var scopedServices = ServicesResolver.Resolve();

            _identityService = scopedServices.GetRequiredService<IIdentityService>();
        }

        [Fact]
        public async Task RegisterAsync_WithExistsEmail_ReturnsResultWithFail()
        {
            var existingEmail = "test1@email.com";

            var result = await _identityService.RegisterAsync(
                _faker.Person.UserName,
                existingEmail,
                _faker.Internet.Password(prefix: "#RSX10"));

            result.Item1.Success.Should().BeFalse();
            result.Item1.Message.Should().Be("Email already exists");
        }

        [Fact]
        public async Task RegisterAsync_WithInvalidPassword_ReturnsResultWithFail()
        {
            var invalidPassword = _faker.Random.AlphaNumeric(15);

            var result = await _identityService.RegisterAsync(
                _faker.Person.UserName,
                _faker.Person.Email,
                invalidPassword);

            result.Item1.Success.Should().BeFalse();
            result.Item1.Message.Split(";").Should().SatisfyRespectively(
                err1 => err1.Should().Be("PasswordRequiresNonAlphanumeric:Passwords must have at least one non alphanumeric character."),
                err2 => err2.Should().Be("PasswordRequiresUpper:Passwords must have at least one uppercase ('A'-'Z')."));
        }

        [Fact]
        public async Task RegisterAsync_WithValidParameters_ReturnsResultWithSuccessAndUserRegistered()
        {
            var result = await _identityService.RegisterAsync(
                _faker.Person.UserName,
                _faker.Person.Email,
                _faker.Internet.Password(length: 15, prefix: "#RSX12"));

            result.Item1.Success.Should().BeTrue();
            result.Item1.Message.Should().Be("Registered successfully");
            result.Item3.Should().NotBeNullOrEmpty();
        }
    }
}
