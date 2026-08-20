using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Dtos.ResponseDto;

namespace Tazkarti.Application.Interfaces
{
	public interface IAuthService
	{
		public Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto);

		public Task<BaseResult<string>> RegisterAsync(RegisterDto registerDto);
	}
}
