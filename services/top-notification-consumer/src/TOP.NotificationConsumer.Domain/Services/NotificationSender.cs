using MailKit.Net.Smtp;
using NETCore.MailKit.Core;
using System;
using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Constants;
using TOP.NotificationConsumer.Domain.Interfaces.Domain;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;
using TOP.NotificationConsumer.Domain.Models.Message;

namespace TOP.NotificationConsumer.Domain.Services
{
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
                _logWriter.Error($"email sended to {args.Email}", args);
            }
            catch (SmtpCommandException smtpEx)
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
