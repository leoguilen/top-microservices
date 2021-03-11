using TOP.ProfileService.Domain.ValueObject;

namespace TOP.ProfileService.Application.Models
{
    public class UserProfileDetailsResponse
    {
        public Address Address { get; set; }
        public string Bio { get; set; }
        public byte[] ProfileImage { get; set; }
    }
}
