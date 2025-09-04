using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMovieDatabase.Data;
using WebMovieDatabase.Models;

namespace WebMovieDatabase.Controllers;


public class MoviesController(ApplicationDbContext context) : Controller
{

    [Authorize]
    public async Task<IActionResult> AddMovie()
    {
        var allActors = await context.Actors.ToListAsync();

        var viewModel = new AddMovieViewModel
        {
            // populate the list of available actors for the dropdown
            AvailableActors = allActors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMovie(AddMovieViewModel model)
    {
        if (ModelState.IsValid)
        {
            var movie = model.Movie;

            if (model.SelectedActorIds.Count > 0)
            {
                // get the actors with the given ids and assign them to the movie
                var actors = await context.Actors.Where(a => model.SelectedActorIds.Contains(a.Id)).ToListAsync();
                movie.Actors = actors;
            }

            // saves the movie and redirects user back to the index page
            context.Movies.Add(movie);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id == 0)
            return Content("Invalid movie id");

        // fetches the movie with the given id
        // also ensures to include the actors in the movie
        var movie = await context.Movies
            .Include(m => m.Actors!)
            .Include(m => m.Ratings!)
            .ThenInclude(r => r.User)
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
        if (id == 0)
            return Content("Invalid movie id");

        var movie = await context.Movies.FindAsync(id);
        if (movie != null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        // TODO: implement not found page
        if (id == 0)
            return Content("Invalid movie id");

        // fetches the movie with the given id
        // ensures to include the actors in the movie
        var movie = await context.Movies.Where(m => m.Id == id).Include(m => m.Actors).FirstOrDefaultAsync();

        if (movie == null)
        {
            return Content("Movie not found");
        }

        var staringActorIds = movie.Actors.Select(a => a.Id).ToList();
        var allActors = await context.Actors.ToListAsync();

        // create a view model with the movie and the list of actors
        var viewModel = new AddMovieViewModel
        {
            Movie = movie,
            SelectedActorIds = staringActorIds,
            // populate the list of available actors for the dropdown
            AvailableActors = allActors.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Selected = staringActorIds.Contains(a.Id),
                Text = a.Name
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AddMovieViewModel model)
    {
        if (ModelState.IsValid)
        {
            // fetch the updated movie from the database
            var movie = await context.Movies
                .Where(m => m.Id == model.Movie.Id)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            // update the movie properties
            movie.Title = model.Movie.Title;
            movie.Description = model.Movie.Description;
            movie.ImageUrl = model.Movie.ImageUrl;

            // replace the collection of actors
            var newActors = await context.Actors
                .Where(a => model.SelectedActorIds.Contains(a.Id))
                .ToListAsync();
            movie.Actors = newActors;

            // save changes and go back to index page
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(model);
    }

    public async Task<IActionResult> Index()
    {
        var movies = await context.Movies.Include(m => m.Ratings).ToListAsync();
        return View(movies);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}