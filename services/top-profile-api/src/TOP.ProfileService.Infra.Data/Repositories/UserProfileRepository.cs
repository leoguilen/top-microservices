using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Interfaces.Infra;
using TOP.ProfileService.Infra.Data.Context;

namespace TOP.ProfileService.Infra.Data.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _dbContext;

        public UserProfileRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetUserProfileAsync(Guid userId)
        {
            return await _dbContext.UserProfiles
                .AsNoTracking()
                .Include(x => x.UserProfileDetails)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<int> UpdateUserProfileAsync(Guid userId, UserProfile userProfile)
        {
            var userProfileExistent = await _dbContext.UserProfiles
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (userProfileExistent is null)
            {
                return 0;
            }

            userProfileExistent.FirstName = userProfile.FirstName ?? userProfileExistent.FirstName;
            userProfileExistent.LastName = userProfile.LastName ?? userProfileExistent.LastName;
            userProfileExistent.BirthDate = userProfile.BirthDate ?? userProfileExistent.BirthDate;
            userProfileExistent.AcademicLevel = userProfile.AcademicLevel ?? userProfileExistent.AcademicLevel;
            userProfileExistent.UpdatedAt = DateTime.Now;

            var updated = _dbContext.Update(userProfileExistent);
            var affectedRows = await _dbContext.SaveChangesAsync();

            return affectedRows;
        }

        public async Task<int> UpdateUserProfileDetailsAsync(Guid userId, UserProfileDetails userProfileDetails)
        {
            var userProfileDetailsExistent = await _dbContext.UserProfileDetails
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (userProfileDetailsExistent is null)
            {
                return 0;
            }

            userProfileDetailsExistent.Address = userProfileDetails.Address ?? userProfileDetailsExistent.Address;
            userProfileDetailsExistent.Bio = userProfileDetails.Bio ?? userProfileDetailsExistent.Bio;
            userProfileDetailsExistent.ProfileImage = userProfileDetails.ProfileImage ?? userProfileDetailsExistent.ProfileImage;

            var updated = _dbContext.Update(userProfileDetailsExistent);
            var affectedRows = await _dbContext.SaveChangesAsync();

            return affectedRows;
        }
    }
}
