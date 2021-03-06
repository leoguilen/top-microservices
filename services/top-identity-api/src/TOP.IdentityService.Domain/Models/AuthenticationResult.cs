namespace TOP.IdentityService.Domain.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
