namespace Shared.Helper.PdfGenerator
{
	public record MatchTazkaraDto
	{
		public string? BookingReference { get; set; }
		public string? HomeTeamName { get; set; }

		public string? AwayTeamName { get; set; }

		public string? CompetitionName { get; set; }

		public string FanId { get; set; }

		public string HolderName { get; set; }

		public decimal Price { get; set; }

		public string? Gate { get; set; }

		public bool? IsActive { get; set; }
		public string? Round { get; set; }

		public string? Title { get; set; }

		//public string? QrCodeImageBase64 { get; set; }
	}
}