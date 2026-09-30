using System;
using System.Collections.Generic;
using System.Text;
using Tazkarti.Application.Dtos.RequestDto;

namespace Tazkarti.Application.Email
{
	public interface IEmail
	{
		Task SendEmail(EmailDto emailDto);
	}
}
