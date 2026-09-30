using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tazkarti.Application.Dtos.RequestDto;
using Tazkarti.Application.Email;

namespace Tazkarti.Application.RabbitMQ.Consumers
{
	public class EmailConsumer : BackgroundService
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public EmailConsumer(IConfiguration configuration,
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
				queue: "EmailQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.ReceivedAsync += async (sender, arg) =>
			{
				var body = arg.Body.ToArray();

				var json = Encoding.UTF8.GetString(body);

				var message = JsonSerializer.Deserialize<EmailEvent>(json);

				if (message != null)
				{
					using var scope = _serviceScopeFactory.CreateScope();

					var emailService = scope.ServiceProvider.GetRequiredService<IEmail>();

					var emailDto = new EmailDto
					{
						To = message.To ?? string.Empty,
						Subject = message.Subject ?? "Your Tazkarti Ticket",
						Body = message.Body ?? "<h3>Thank you for booking with Tazkarti!</h3><p>Please find your ticket attached.</p>",
						AttachmentPath = message.TazkaraPdfPath
					};

					await emailService.SendEmail(emailDto);
				}

				await channel.BasicAckAsync(arg.DeliveryTag, false);
			};

			await channel.BasicConsumeAsync(
				queue: "EmailQueue",
				autoAck: false,
				consumer: consumer
			);

			await Task.Delay(
				Timeout.InfiniteTimeSpan,
				stoppingToken);
		}
	}
}
