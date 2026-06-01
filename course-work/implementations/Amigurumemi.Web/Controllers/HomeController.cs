using Amigurumemi.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Amigurumemi.Web.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Route("Home/Error/{statusCode?}")]
		public IActionResult Error(int? statusCode)
		{
			var model = new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
			};

			if (statusCode.HasValue)
			{
				if (statusCode == 404)
				{
					ViewBag.ErrorMessage = "Oops! The page you are looking for does not exist.";
					return View("NotFound"); 
				}
				if (statusCode == 403)
				{
					ViewBag.ErrorMessage = "Access Denied. You do not have permission to view this resource.";
					return View("AccessDenied");
				}
			}

			ViewBag.ErrorMessage = "An unexpected error occurred on our server. Please try again later.";
			return View(model);
		}
	}
}
