namespace Tazkarti.Application.Email
{
	public class EmailEvent
	{
		public string? TazkaraPdfPath { get; set; }
		public string? To { get; set; }
		public string? Subject { get; set; }
		public string? Body { get; set; }
	}
}
