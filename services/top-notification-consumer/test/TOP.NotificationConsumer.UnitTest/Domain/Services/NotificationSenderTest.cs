using Bogus;
using MailKit.Net.Smtp;
using Moq;
using NETCore.MailKit.Core;
using System;
using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Constants;
using TOP.NotificationConsumer.Domain.Interfaces.Domain;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;
using TOP.NotificationConsumer.Domain.Models.Message;
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
            //_emailServiceMock.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(() => throw new SmtpCommandException(
            //        SmtpErrorCode.UnexpectedStatusCode,
            //        SmtpStatusCode.ErrorInProcessing,
            //        "erro"));
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

        [Fact]
        public void Handle_WhenMessageHasInvalidEmail_ThrowException()
        {
            var actionType = RoutingKey.RegisterUser;
            var userMessage = new UserMessage
            {
                Id = _faker.Random.Guid().ToString(),
                Email = _faker.Random.String2(15, 20),
                UserName = _faker.Person.UserName,
                PhoneNumber = _faker.Person.Phone,
                Token = _faker.Random.AlphaNumeric(25)
            };

            Assert.Throws<SmtpCommandException>(() =>
            {
                var result = _sender
                    .SetAction(actionType)
                    .Handle(userMessage);
            });
        }
    }

    public class NotificationSender : INotificationSender
    {
        private readonly IEmailService _sender;
        private readonly ILogWriter _logWriter;

        private string _subject;
        private string _message;

        public NotificationSender(
            IEmailService sender,
            ILogWriter logWriter)
        {
            _sender = sender;
            _logWriter = logWriter;
        }

        public INotificationSender SetAction(string actionType)
        {
            switch (actionType)
            {
                case RoutingKey.RegisterUser:
                    {
                        _subject = "Novo usuário registrado";
                        _message = $"Bem vindo!\nPara confirmar o seu registro utilize o token abaixo\nToken: @token";
                    }
                    break;
                case RoutingKey.ResetUserPassword:
                    {
                        _subject = "Solicitação para resetar senha";
                        _message = $"Recebemos sua solicitação para resetar sua senha!\n!Para confirmar da nova senha use o token abaixo:\nToken: @token";
                    }
                    break;
                default: 
                    throw new ArgumentException("action is not exists", nameof(actionType));
            }

            return this;
        }

        public async Task Handle(UserMessage args)
        {
            try
            {
                await _sender.SendAsync(args.Email, _subject, _message, false);
            }
            catch(SmtpCommandException smtpEx)
            {
                _logWriter.Error(smtpEx.Message, args, smtpEx);
                throw;
            }
            catch (Exception ex)
            {
                _logWriter.Error("error sending email", args, ex);
                throw;
            }
        }
    }
}
