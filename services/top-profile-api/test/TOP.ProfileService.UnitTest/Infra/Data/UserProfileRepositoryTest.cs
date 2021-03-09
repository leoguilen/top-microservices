using System;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Enums;
using TOP.ProfileService.Domain.Interfaces.Infra;
using TOP.ProfileService.Infra.Data.Repositories;
using Xunit;

namespace TOP.ProfileService.UnitTest.Infra.Data
{
    public class UserProfileRepositoryTest : MockData
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileRepositoryTest()
        {
            _userProfileRepository = new UserProfileRepository(DbCtx);
        }

        [Fact]
        public async Task GetUserProfileAsync_WithExistentId_ReturnsUserProfile()
        {
            var userId = Guid.Parse("21DCE4AF-7DFF-4D11-8BD4-2EAC339282A4");

            var userProfileResult = await _userProfileRepository
                .GetUserProfileAsync(userId);

            Assert.Equal(userId, userProfileResult.Id);
        }

        [Fact]
        public async Task GetUserProfileAsync_WithNonExistentId_ReturnsNull()
        {
            var userId = Guid.NewGuid();

            var userProfileResult = await _userProfileRepository
                .GetUserProfileAsync(userId);

            Assert.Null(userProfileResult);
        }

        [Fact]
        public async Task UpdateUserProfileAsync_WithExistentId_UpdateUserProfile()
        {
            var userId = Guid.Parse("21DCE4AF-7DFF-4D11-8BD4-2EAC339282A4");
            var newUserProfile = new UserProfile
            {
                LastName = "Updated",
                AcademicLevel = AcademicLevel.HighSchool
            };

            var result = await _userProfileRepository
                .UpdateUserProfileAsync(userId, newUserProfile);

            Assert.Equal(1, result);

            var updatedUserProfile = await _userProfileRepository
                .GetUserProfileAsync(userId);
            Assert.Equal(newUserProfile.LastName, updatedUserProfile.LastName);
            Assert.Equal(newUserProfile.AcademicLevel, updatedUserProfile.AcademicLevel);
            Assert.Equal(DateTime.Now.ToLongDateString(), updatedUserProfile.UpdatedAt.ToLongDateString());
        }

        [Fact]
        public async Task UpdateUserProfileAsync_WithNonExistentId_NotUpdatedUserProfile()
        {
            var userId = Guid.NewGuid();
            var newUserProfile = new UserProfile
            {
                LastName = "Updated",
                AcademicLevel = AcademicLevel.HighSchool
            };

            var result = await _userProfileRepository
                .UpdateUserProfileAsync(userId, newUserProfile);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task UpdateUserProfileDetailsAsync_WithExistentId_UpdateUserProfileDetails()
        {
            var userId = Guid.Parse("21DCE4AF-7DFF-4D11-8BD4-2EAC339282A4");
            var newUserProfileDetails = new UserProfileDetails
            {
                Bio = "bio updated",
            };

            var result = await _userProfileRepository
                .UpdateUserProfileDetailsAsync(userId, newUserProfileDetails);

            Assert.Equal(1, result);

            var updatedUserProfile = await _userProfileRepository
                .GetUserProfileAsync(userId);
            Assert.Equal(newUserProfileDetails.Bio, updatedUserProfile.UserProfileDetails.Bio);
        }

        [Fact]
        public async Task UpdateUserProfileDetailsAsync_WithNonExistentId_NotUpdatedUserProfileDetails()
        {
            var userId = Guid.NewGuid();
            var newUserProfileDetails = new UserProfileDetails
            {
                Bio = "bio updated",
            };

            var result = await _userProfileRepository
                .UpdateUserProfileDetailsAsync(userId, newUserProfileDetails);

            Assert.Equal(0, result);
        }
    }
}
