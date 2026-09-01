using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.ResponseDto
{
	public record VenuesDto
	{
		public string Name { get; set; } = null!;
		public int Capacity { get; set; }
		public string Location { get; set; } = null!;
		public string City { get; set; } = null!;
		public string? MetroAccess { get; set; }
		public string? ImageUrl { get; set; }
		public string? Description { get; set; }

		public IReadOnlyList<string>? GateName { get; set; } = null!;
		public IReadOnlyList<string>? AllocatedFor { get; set; } = null!;
	}
}
