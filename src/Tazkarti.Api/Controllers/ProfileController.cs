using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Features.Profile.Query;

namespace Tazkarti.Api.Controllers;

public class ProfileController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<BaseResult<ProfileDto>>> GetProfile()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new GetProfileQuery(userId));

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }
}
