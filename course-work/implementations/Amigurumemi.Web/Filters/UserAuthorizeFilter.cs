using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class UserAuthorizeAttribute : TypeFilterAttribute
{
	public UserAuthorizeAttribute() : base(typeof(UserAuthorizeFilter))
	{
	}

	private class UserAuthorizeFilter : IAuthorizationFilter
	{
		private readonly ITempDataDictionaryFactory _tempDataFactory;

		public UserAuthorizeFilter(ITempDataDictionaryFactory tempDataFactory)
		{
			_tempDataFactory = tempDataFactory;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var userId = context.HttpContext.Session.GetString("UserId");

			if (string.IsNullOrEmpty(userId))
			{
				var tempData = _tempDataFactory.GetTempData(context.HttpContext);
				tempData["ErrorMessage"] = "You need to be logged in to access this page.";

				context.Result = new RedirectToActionResult("Login", "Account", null);
			}
		}
	}
}