using Microsoft.AspNetCore.Identity;

namespace TOP.IdentityService.Domain.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
