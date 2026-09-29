using Shared.Helper.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tazkarti.Application.RabbitMQ
{
	public interface ITazkaraPdfProducer
	{
		Task PublishTazkaraAsync(MatchTazkaraDto tazkaraDto);
	}
}
