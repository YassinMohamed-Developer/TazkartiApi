using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public record ChatRequest
	{
		public string Message { get; set; } = null!;
	}
}
