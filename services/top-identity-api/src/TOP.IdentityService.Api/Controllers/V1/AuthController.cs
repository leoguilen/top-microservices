using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TOP.IdentityService.Application.UseCases.ConfirmEmail;
using TOP.IdentityService.Application.UseCases.Login;
using TOP.IdentityService.Application.UseCases.PasswordRecovery;
using TOP.IdentityService.Application.UseCases.Register;
using TOP.IdentityService.Application.UseCases.ResetPassword;

namespace TOP.IdentityService.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var authResult = await _mediator.Send(request);

            if (authResult.Success is false)
            {
                if (authResult.Message is "User not found")
                {
                    return NotFound(new
                    {
                        authResult.Success,
                        authResult.Message
                    });
                }

                return BadRequest(new
                {
                    authResult.Success,
                    authResult.Message
                });
            }

            return Ok(authResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            var authResult = await _mediator.Send(request);

            if (authResult.Success is false)
            {
                return BadRequest(new
                {
                    authResult.Success,
                    authResult.Message
                });
            }

            return Ok(authResult);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
        {
            var authResult = await _mediator.Send(request);

            if (authResult.Success is false)
            {
                if (authResult.Message is "Email not found")
                {
                    return NotFound(new
                    {
                        authResult.Success,
                        authResult.Message
                    });
                }

                return BadRequest(new
                {
                    authResult.Success,
                    authResult.Message
                });
            }

            return Ok(authResult.Message);
        }

        [HttpPost("passwordRecovery")]
        public async Task<IActionResult> PasswordRecovery([FromBody] PasswordRecoveryCommand request)
        {
            var authResult = await _mediator.Send(request);

            if (authResult.Success is false)
            {
                if (authResult.Message is "Email not found")
                {
                    return NotFound(new
                    {
                        authResult.Success,
                        authResult.Message
                    });
                }

                return BadRequest(new
                {
                    authResult.Success,
                    authResult.Message
                });
            }

            return Ok(authResult.Message);
        }

        [HttpPost("passwordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] ResetPasswordCommand request)
        {
            var authResult = await _mediator.Send(request);

            if (authResult.Success is false)
            {
                if (authResult.Message is "User not found")
                {
                    return NotFound(new
                    {
                        authResult.Success,
                        authResult.Message
                    });
                }

                return BadRequest(new
                {
                    authResult.Success,
                    authResult.Message
                });
            }

            return Ok(authResult);
        }
    }
}
