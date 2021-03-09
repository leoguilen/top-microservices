using System;

namespace TOP.ProfileService.Domain.Configurations
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
