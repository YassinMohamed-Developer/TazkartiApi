using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.ResponseDto
{
	public class ProfileDto
	{
		public string Id { get; set; } = null!;
		public string FanId { get; set; } = null!;
		public string FullName { get; set; } = null!;
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public DateTime DateOfBirth { get; set; }
		public Gender Gender { get; set; }
		public Governorate Governorate { get; set; }
		public Nationality Nationality { get; set; }
		public string? AvatarUrl { get; set; }
		public LoyaltyTier LoyaltyTier { get; set; }
		public int AttendancePoints { get; set; }
		public string? FavouriteClubName { get; set; }
	}
}
