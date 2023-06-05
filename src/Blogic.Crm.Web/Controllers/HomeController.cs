using System.Diagnostics;
using Blogic.Crm.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Web.Controllers;

public sealed class HomeController : Controller
{
	[HttpGet]
	[Route("/")]
	[Route("/Index")]
	[Route("/Home")]
	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	[Route("/Privacy")]
	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}