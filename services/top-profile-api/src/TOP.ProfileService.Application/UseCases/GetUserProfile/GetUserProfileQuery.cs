using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TOP.ProfileService.Application.Models;
using TOP.ProfileService.Domain.Interfaces.Infra;

namespace TOP.ProfileService.Application.UseCases.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileResponse>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository
                .GetUserProfileAsync(request.UserId);

            var userProfileResponse = _mapper
                .Map<UserProfileResponse>(userProfile);

            return userProfileResponse;
        }
    }
}
