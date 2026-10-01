namespace Tazkarti.Application.RabbitMQ
{
	public class EmailEvent
	{
		public string? To { get; set; }
		public string? Subject { get; set; }
		public string? Body { get; set; }
		public string? TazkaraPdfPath { get; set; }
	}
}
