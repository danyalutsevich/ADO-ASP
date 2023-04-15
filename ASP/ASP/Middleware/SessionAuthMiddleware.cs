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
			//string userId = context.Session.GetString("userId");

			//dataContext.Users.Find(Guid.Parse(userId));

			logger.LogInformation("SessionAuthMiddleware");
			await _next(context);
		}
	}
}
