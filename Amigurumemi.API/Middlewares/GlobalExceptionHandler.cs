using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Amigurumemi.Api.Middlewares
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			_logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

			var problemDetails = new ProblemDetails
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = "https://datatracker.ietf.org/doc/html/rfc7807",
				Title = "An unexpected error occurred",
				Detail = exception.Message, 
				Instance = httpContext.Request.Path
			};

			if (exception is KeyNotFoundException || exception.Message.Contains("not found"))
			{
				problemDetails.Status = (int)HttpStatusCode.NotFound;
				problemDetails.Title = "Resource Not Found";
			}

			httpContext.Response.StatusCode = problemDetails.Status.Value;
			httpContext.Response.ContentType = "application/problem+json";

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}