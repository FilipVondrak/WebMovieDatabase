using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMovieDatabase.Data;
using WebMovieDatabase.Models;

namespace WebMovieDatabase.Controllers;


public class MoviesController(ApplicationDbContext context) : Controller
{
    [Authorize]
    public IActionResult AddMovie()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMovie(Movie movie)
    {
        if (ModelState.IsValid)
        {
            await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        // fetches the movie with the given id
        // also ensures to include the actors in the movie
        var movie = await context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await context.Movies.FindAsync(id);
        if (movie != null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var movies = await context.Movies.ToListAsync();
        return View(movies);
    }
}