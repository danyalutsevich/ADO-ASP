using ASP.Data;

namespace ASP.Middleware
{
	public class SessionAuthMiddleware
	{
		private readonly RequestDelegate _next;

		public SessionAuthMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(
			HttpContext context,
			ILogger<SessionAuthMiddleware> logger,
			DataContext dataContext
			)
		{
			string userId = context.Session.GetString("userId");
			try
			{
				var user = dataContext.Users.Find(Guid.Parse(userId));
				if (user is not null)
				{
					context.Items.Add("user", user);
				}
			}
			catch (Exception e)
			{
				logger.LogError(e.Message, "Error in SessionAuthMiddleware");
			}


			logger.LogInformation("SessionAuthMiddleware");
			await _next(context);
		}

	}

	public static class SessionAuthMiddlewareExtension
	{
		public static IApplicationBuilder UseSessionAuth(this IApplicationBuilder app)
		{
			return app.UseMiddleware<SessionAuthMiddleware>();
		}
	}
}
