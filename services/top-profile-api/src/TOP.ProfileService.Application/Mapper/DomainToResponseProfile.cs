using AutoMapper;
using TOP.ProfileService.Application.Models;
using TOP.ProfileService.Domain.Entities;

namespace TOP.ProfileService.Application.Mapper
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile ()
        {
            CreateMap<UserProfileDetails, UserProfileDetailsResponse> ();
            CreateMap<UserProfile, UserProfileResponse> ()
                .ForMember (x => x.UserId, y => y.MapFrom (z => z.Id.ToString ()))
                .ForMember (x => x.AcademicLevel, y => y.MapFrom (z => z.AcademicLevel.HasValue ? z.AcademicLevel.Value.ToString() : null))
                .ForMember (x => x.Details, y => y.MapFrom (z => z.UserProfileDetails));
        }
    }
}