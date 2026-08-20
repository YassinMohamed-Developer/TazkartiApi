using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
	public class BaseResult<T>
	{
		public bool IsSuccess { get; set; } = true;

		public string Message { get; set; } = string.Empty;
		public T Data { get; set; }

		public List<string> Errors { get; set; }

		public int StatusCode { get; set; }

		public BaseResult()
		{

		}
		public BaseResult(string message)
		{
			Message = message;
		}
	}
}
