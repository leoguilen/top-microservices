namespace TOP.IdentityService.Domain.Configurations
{
    public class RabbitMQConfiguration
    {
        public string ExchangeName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string RoutingKey { get; set; }
        public string NotificationQueue { get; set; }
        public string SincronizationQueue { get; set; }
    }
}
