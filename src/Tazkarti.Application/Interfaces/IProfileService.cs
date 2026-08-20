using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Dtos.ResponseDto;

namespace Tazkarti.Application.Interfaces
{
	public interface IProfileService
	{
		public Task<BaseResult<ProfileDto>> GetProfileAsync(string userId);
	}
}
