using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.Helper;
using System;
using System.IO;
using System.Threading.Tasks;
using Tazkarti.Application.Dtos.RequestDto;

namespace Tazkarti.Application.Email
{
	public class Email : IEmail
	{
		private MailSettingsOptions _options;

		public Email(IOptions<MailSettingsOptions> options)
		{
			_options = options.Value;
		}

		public async Task SendEmail(EmailDto emailDto)
		{
			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_options.Email),
				Subject = emailDto.Subject,
			};

			mail.To.Add(MailboxAddress.Parse(emailDto.To));
			mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

			var builder = new BodyBuilder();
			builder.HtmlBody = emailDto.Body;

			if (!string.IsNullOrWhiteSpace(emailDto.AttachmentPath) && File.Exists(emailDto.AttachmentPath))
			{
				builder.Attachments.Add(emailDto.AttachmentPath);
			}

			mail.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();

			await smtp.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);

			await smtp.AuthenticateAsync(_options.Email, _options.Password);

			await smtp.SendAsync(mail);

			await smtp.DisconnectAsync(true);
		}
	}
}
