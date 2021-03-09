using System;
using TOP.ProfileService.Domain.Entities.Base;
using TOP.ProfileService.Domain.Enums;

namespace TOP.ProfileService.Domain.Entities
{
    public class UserProfile : Entity
    {
        public UserProfile() 
        {
            Id = Guid.NewGuid();
        }

        public UserProfile(string id)
        {
            Id = Guid.Parse(id);
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AcademicLevel? AcademicLevel { get; set; }

        public virtual UserProfileDetails UserProfileDetails { get; set; }
    }
}
