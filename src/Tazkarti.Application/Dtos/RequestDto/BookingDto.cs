using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public record BookingDto
	{
		public BookingType BookingType { get; set; }

		public int? MatchId { get; set; }

		public int? EventId { get; set; }

		public int? CategoryId { get; set; }

		public int? TierId { get; set; }

		public int VenueId { get; set; }

		public string City { get; set; } = null!;

		public string? Gate { get; set; }

		public string? Block { get; set; }

		public decimal TotalAmount { get; set; }

		public int Quantity { get; set; }

		public PaymentMethod PaymentMethod { get; set; }


	}
}
