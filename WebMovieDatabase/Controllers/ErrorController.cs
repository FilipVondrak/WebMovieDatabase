using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMovieDatabase.Models;

namespace WebMovieDatabase.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult MovieNotFound() => View();

    public IActionResult InvalidId() => View();
}