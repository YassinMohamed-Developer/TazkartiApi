using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Shared.Helper.PdfGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Email;

namespace Tazkarti.Application.RabbitMQ
{
	public class RabbitMQProducer : IRabbitMQProducer
	{
		private readonly IConfiguration _configuration;

		public RabbitMQProducer(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task PublishTazkaraAsync(MatchTazkaraDto tazkaraDto)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:Host"],
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "TazkaraQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			await channel.ExchangeDeclareAsync(

				exchange: "TazkaraExchange",
				type: ExchangeType.Direct,
				durable: true,
				autoDelete: false
			);

			await channel.QueueBindAsync(
				queue: "TazkaraQueue",
				exchange: "TazkaraExchange",
				routingKey: "Tazkara_RoutingKey"
			);

			var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(tazkaraDto));

			await channel.BasicPublishAsync(
				exchange: "TazkaraExchange",
				routingKey: "Tazkara_RoutingKey",
				body: body
			);
		}

		public async Task PublishEmailAsync(EmailEvent emailEvent)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:Host"],
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "EmailQueue",
				durable: true,
				autoDelete: false,
				exclusive: false
			);

			await channel.ExchangeDeclareAsync(
				exchange: "EmailExchange",
				type: ExchangeType.Direct,
				durable: true,
				autoDelete: false
			);

			await channel.QueueBindAsync(
				queue: "EmailQueue",
				exchange: "EmailExchange",
				routingKey: "Email_RoutingKey"
			);

			var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(emailEvent));

			await channel.BasicPublishAsync(
				exchange: "EmailExchange",
				routingKey: "Email_RoutingKey",
				body: body
			);
		}
	}
}
