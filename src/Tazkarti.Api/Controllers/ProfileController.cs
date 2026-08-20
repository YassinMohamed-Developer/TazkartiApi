using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using Tazkarti.Application.Dtos.ResponseDto;
using Tazkarti.Application.Interfaces;

namespace Tazkarti.Api.Controllers
{
	public class ProfileController : BaseApiController
	{
		private readonly IProfileService _profileService;

		public ProfileController(IProfileService profileService)
		{
			_profileService = profileService;	
		}

		[Authorize]
		[HttpGet()]
		public async Task<ActionResult<BaseResult<ProfileDto>>> GetProfile()
		{

			var userid = User.FindFirst("UserId").Value;

			var result = await _profileService.GetProfileAsync(userid);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
