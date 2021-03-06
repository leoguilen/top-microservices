namespace TOP.NotificationConsumer.Domain.Models.Message
{
    public class UserMessage
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}
