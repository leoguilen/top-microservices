using System.Threading.Tasks;
using TOP.NotificationConsumer.Domain.Interfaces.Infra;
using TOP.NotificationConsumer.Domain.Models;

namespace TOP.NotificationConsumer.Test.Infra.Services
{
    public class MailServiceTest
    {
        private readonly IMailService _mailService;

        public MailServiceTest()
        {
            _mailService = new MailService();
        }

        [Fact]
    }

    public class MailService : IMailService
    {
        public Task SendMail(NotificationArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
