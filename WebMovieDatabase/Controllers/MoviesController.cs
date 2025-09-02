using Microsoft.AspNetCore.Mvc;

namespace WebMovieDatabase.Controllers;

public class MoviesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}