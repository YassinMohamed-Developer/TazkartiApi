using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Helper.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Tazkarti.Application.RabbitMQ.Consumers
{
	public class TazkaraPdfConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public TazkaraPdfConsumer(IConfiguration configuration,
			IServiceScopeFactory serviceScopeFactory)
		{
			_configuration = configuration;
			_serviceScopeFactory = serviceScopeFactory;
		}

		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var factory = new ConnectionFactory
			{
				HostName = _configuration["RabbitMQ:Host"]
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "TazkaraQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.ReceivedAsync += async (sender, arg) =>
			{
				var body = arg.Body.ToArray();

				var json = Encoding.UTF8.GetString(body);

				var message = JsonSerializer.Deserialize<MatchTazkaraDto>(json);

				using var scope = _serviceScopeFactory.CreateScope();

				var pdfservice = scope.ServiceProvider.GetRequiredService<ITazkaraPdf>();

				var pdfBytes = pdfservice.GeneratePdf(message!);

				Directory.CreateDirectory("Tickets");

				var filePath =
					Path.Combine(
						"Tickets",
						$"Ticket-{message!.BookingReference}.pdf");

				await File.WriteAllBytesAsync(filePath, pdfBytes);

				await channel.BasicAckAsync(arg.DeliveryTag, false);
			};

			await channel.BasicConsumeAsync(
				queue: "TazkaraQueue",
				autoAck: false,
				consumer: consumer
				);

			await Task.Delay(
			Timeout.InfiniteTimeSpan.Seconds,
			stoppingToken);
		}
	}
}
