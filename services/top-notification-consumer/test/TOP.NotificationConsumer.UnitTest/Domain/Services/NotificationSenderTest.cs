using Bogus;
using Moq;
using NETCore.MailKit.Core;
using System;
using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Constants;
using TOP.NotificationConsumer.Domain.Interfaces.Domain;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;
using TOP.NotificationConsumer.Domain.Models.Message;
using TOP.NotificationConsumer.Domain.Services;
using Xunit;

namespace TOP.NotificationConsumer.UnitTest.Domain.Services
{
    public class NotificationSenderTest
    {
        private readonly INotificationSender _sender;
        private readonly Faker _faker = new Faker("pt_BR");

        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private readonly Mock<ILogWriter> _logWriterMock = new Mock<ILogWriter>();

        public NotificationSenderTest()
        {
            _sender = new NotificationSender(_emailServiceMock.Object, _logWriterMock.Object);
        }

        [Fact]
        public void SetAction_WhenActionTypeNotExists_ThrowArgumentException()
        {
            var invalidActionType = _faker.Random.String2(5, 15);

            var message = Assert.Throws<ArgumentException>(() =>
            {
                var sender = _sender.SetAction(invalidActionType);
            }).Message;

            Assert.Equal("action is not exists (Parameter 'actionType')", message);
        }

        [Fact]
        public void Handle_WhenConsumedEventIsValid_NotificationShouldBeSendedToUser()
        {
            var actionType = RoutingKey.RegisterUser;
            var userMessage = new UserMessage
            {
                Id = _faker.Random.Guid().ToString(),
                Email = _faker.Person.Email,
                UserName = _faker.Person.UserName,
                PhoneNumber = _faker.Person.Phone,
                Token = _faker.Random.AlphaNumeric(25)
            };

            var result = _sender
                .SetAction(actionType)
                .Handle(userMessage);

            Assert.True(result.Exception is null);
            Assert.True(result.Status == TaskStatus.RanToCompletion);
        }
    }
}
