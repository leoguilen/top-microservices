using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TOP.ProfileService.Application.Models;
using TOP.ProfileService.Application.UseCases.GetUserProfile;
using TOP.ProfileService.Application.UseCases.UpdateUserProfile;
using TOP.ProfileService.Application.UseCases.UpdateUserProfileDetails;

namespace TOP.ProfileService.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId:Guid}")]
        [ProducesResponseType(typeof(UserProfileResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserProfile([FromRoute] Guid userId)
        {
            var userProfile = await _mediator.Send(
                new GetUserProfileQuery { UserId = userId });

            if (userProfile is null)
            {
                return NotFound(new { message = "user profile not found" });
            }

            return Ok(userProfile);
        }

        [HttpPut("{userId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] Guid userId, [FromBody] UpdateUserProfileCommand request)
        {
            request.UserId = userId;
            var cmdResult = await _mediator.Send(request);

            if (cmdResult is false)
            {
                return BadRequest(new { message = "could not update" });
            }

            return Ok(new { message = "user profile updated" });
        }

        [HttpPut("{userId:Guid}/details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserProfileDetails([FromRoute] Guid userId, [FromForm] UpdateUserProfileDetailsCommand request)
        {
            request.UserId = userId;
            var cmdResult = await _mediator.Send(request);

            if (cmdResult is false)
            {
                return BadRequest(new { message = "could not update" });
            }

            return Ok(new { message = "user profile details updated" });
        }
    }
}
