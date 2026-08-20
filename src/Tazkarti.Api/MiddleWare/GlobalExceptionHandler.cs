using Microsoft.AspNetCore.Diagnostics;
using Shared.Helper;
using System.Net;

namespace Tazkarti.Api.MiddleWare
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, 
			CancellationToken cancellationToken)
		{
			httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			httpContext.Response.ContentType = "application/json";

			var response = new BaseResult<string>() { 
				Message = "An unexpected error occurred.",
				IsSuccess = false,
				StatusCode = (int)HttpStatusCode.InternalServerError };

			await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
			return true;
		}
	}
}
