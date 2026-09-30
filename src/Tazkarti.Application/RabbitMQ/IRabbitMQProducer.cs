using Shared.Helper.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Email;

namespace Tazkarti.Application.RabbitMQ
{
	public interface IRabbitMQProducer
	{
		Task PublishTazkaraAsync(MatchTazkaraDto tazkaraDto);
		Task PublishEmailAsync(EmailEvent emailEvent);
	}
}
