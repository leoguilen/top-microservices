﻿using System;

namespace TOP.ProfileService.Application.Models
{
    public class UserProfileResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicLevel { get; set; }
        public UserProfileDetailsResponse Details { get; set; }
    }
}
