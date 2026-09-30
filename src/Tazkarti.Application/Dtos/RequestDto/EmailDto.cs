using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public class EmailDto
	{
		public string Subject { get; set; } = string.Empty;

		public string Body { get; set; } = string.Empty;

		public string? From { get; set; }

		public string To { get; set; } = string.Empty;

		public string? AttachmentPath { get; set; }
	}
}
