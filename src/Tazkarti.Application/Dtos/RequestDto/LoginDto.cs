using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public class LoginDto
	{
		public string NationalId { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
