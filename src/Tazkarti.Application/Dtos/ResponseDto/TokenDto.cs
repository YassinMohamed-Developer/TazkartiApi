using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.ResponseDto
{
	public class TokenDto
	{
		public string Token { get; set; } = null!;
		public string TokenType { get; set; } = null!;
	}
}
