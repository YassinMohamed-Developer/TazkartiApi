using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public record TicketCategoryDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public int Available { get; set; }
		public string GateAllocation { get; set; } = null!;
	}
}
