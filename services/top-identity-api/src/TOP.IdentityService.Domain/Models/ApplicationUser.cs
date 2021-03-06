using Microsoft.AspNetCore.Identity;

namespace TOP.IdentityService.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }
        public ApplicationUser(string userName) : base(userName) { }
    }
}
