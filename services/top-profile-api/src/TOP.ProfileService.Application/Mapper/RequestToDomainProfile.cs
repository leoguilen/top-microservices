using AutoMapper;
using TOP.ProfileService.Application.UseCases.UpdateUserProfile;
using TOP.ProfileService.Application.UseCases.UpdateUserProfileDetails;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Extensions;

namespace TOP.ProfileService.Application.Mapper
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<UpdateUserProfileDetailsCommand, UserProfileDetails>()
                .ForMember(x => x.ProfileImage, y => y.MapFrom(x => x.ProfileImage.FormFileToByteArray()));
            CreateMap<UpdateUserProfileCommand, UserProfile>()
                .ForMember(x => x.UserName, y => y.Ignore())
                .ForMember(x => x.Email, y => y.Ignore())
                .ForMember(x => x.PhoneNumber, y => y.Ignore());
        }
    }
}
