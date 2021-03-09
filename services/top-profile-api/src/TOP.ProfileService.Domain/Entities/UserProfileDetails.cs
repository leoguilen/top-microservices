using System;
using System.ComponentModel.DataAnnotations.Schema;
using TOP.ProfileService.Domain.ValueObject;

namespace TOP.ProfileService.Domain.Entities
{
    public class UserProfileDetails
    {
        public Guid UserId { get; set; }
        public Address Address { get; set; }
        public string Bio { get; set; }
        public byte[] ProfileImage { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserProfile UserProfile { get; set; }
    }
}
