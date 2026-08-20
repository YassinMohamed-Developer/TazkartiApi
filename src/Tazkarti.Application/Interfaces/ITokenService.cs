using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Application.Interfaces
{
	public interface ITokenService
	{
		public Task<string> GenerateToken(AppUser appUser);
	}
}
