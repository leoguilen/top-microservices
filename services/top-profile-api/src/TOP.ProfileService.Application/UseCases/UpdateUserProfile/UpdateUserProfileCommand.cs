using AutoMapper;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TOP.ProfileService.Domain.Entities;
using TOP.ProfileService.Domain.Interfaces.Infra;

namespace TOP.ProfileService.Application.UseCases.UpdateUserProfile
{
    public class UpdateUserProfileCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? AcademicLevel { get; set; }
    }

    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, bool>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var updatedUserProfile = _mapper
                .Map<UserProfile>(request);

            var result = await _userProfileRepository
                .UpdateUserProfileAsync(request.UserId, updatedUserProfile);

            return result > 0;
        }
    }
}
