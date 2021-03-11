using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Interfaces.Infra;
using TOP.ProfileService.Domain.ValueObject;

namespace TOP.ProfileService.Application.UseCases.UpdateUserProfileDetails
{
    public class UpdateUserProfileDetailsCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Address Address { get; set; }
        public string Bio { get; set; }
        public IFormFile ProfileImage { get; set; }
    }

    public class UpdateUserProfileDetailsCommandHandler : IRequestHandler<UpdateUserProfileDetailsCommand, bool>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfileDetailsCommandHandler(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateUserProfileDetailsCommand request, CancellationToken cancellationToken)
        {
            var updatedUserProfileDetails = _mapper
                .Map<UserProfileDetails>(request);

            var result = await _userProfileRepository
                .UpdateUserProfileDetailsAsync(request.UserId, updatedUserProfileDetails);

            return result > 0;
        }
    }
}
