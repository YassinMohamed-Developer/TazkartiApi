using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Domain.Entities
{
	public class ChatMessage
	{
		public int Id { get; set; }
		public string UserId { get; set; }

		public string Role { get; set; }
		public string Message { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
