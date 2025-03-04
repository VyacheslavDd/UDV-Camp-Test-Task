using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoArt.Infrastructure.Middlewares
{
	public class ExceptionMiddleware(RequestDelegate next, ILogger logger)
	{
		private readonly RequestDelegate _next = next;
		private readonly ILogger _logger = logger;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception exc)
			{
				await HandleError(exc, context);
			}
		}

		private async Task HandleError(Exception exc, HttpContext context)
		{
			_logger.Error("Exception {@message} happened in {@source}", exc.Message, exc.Source);
			_logger.Information("Inner exception is {@inner}", exc.InnerException);
			_logger.Information("Stack trace: {@trace}", exc.StackTrace);
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync("Неожиданная ошибка... Попробуйте позже!");
		}
	}
}
