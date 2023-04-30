using System.Security.Claims;
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
			logger.LogWarning($"userId: {userId}");
			try
			{
				var user = dataContext.Users.Find(Guid.Parse(userId));
				if (user is not null)
				{
					logger.LogInformation(user.Email);
					context.Items.Add("user", user);
					
					Claim[] claims = new Claim[]
					{
						new Claim(ClaimTypes.Sid, userId),
						new Claim(ClaimTypes.Name, user.Username),
						new Claim(ClaimTypes.NameIdentifier, user.Email),
						new Claim(ClaimTypes.UserData, user.Avatar ?? String.Empty)
					};

					var principal = new ClaimsPrincipal(
						new ClaimsIdentity(claims, nameof(SessionAuthMiddleware)));
					context.User = principal;

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
