using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tazkarti.Domain.Enums;

namespace Tazkarti.Application.Dtos.RequestDto
{
	public class RegisterDto
	{
		[Required]
		[StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be 14 digits.")]
		[RegularExpression("^[0-9]{14}$", ErrorMessage = "National ID must be 14 digits.")]
		public string NationalId { get; set; } = null!;

		[Required]
		public string NationalName { get; set; } = null!;

		[Required]
		public DateTime DateOfBirth { get; set; }

		[Required]
		public Gender Gender { get; set; }

		[Required]
		[RegularExpression(@"^01[0125]\d{8}$",ErrorMessage = "Please enter a valid Egyptian phone number.")]
		public string PhoneNumber { get; set; } = null!;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		[RegularExpression("^[A-Z][A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{5,}$")]
		public string Password { get; set; } = null!;

		[Required]
		public Governorate Governorate { get; set; }

		[Required]
		public int FavouriteClubId { get; set; }

		public string? AvatarUrl { get; set; }
	}
}
