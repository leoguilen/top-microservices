using System;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Domain.Interfaces.Infra
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileAsync(Guid userId);
        Task<int> UpdateUserProfileAsync(Guid userId, UserProfile userProfile);
        Task<int> UpdateUserProfileDetailsAsync(Guid userId, UserProfileDetails userProfileDetails);
    }
}
