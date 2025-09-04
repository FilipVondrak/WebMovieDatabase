using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebMovieDatabase.Data;
using WebMovieDatabase.Models;

namespace WebMovieDatabase.Controllers;

[Authorize]
public class RatingsController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager)

    : Controller {

    public async Task<IActionResult> AddRating(int id)
    {
        var movie = await context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        var model = new AddRatingViewModel()
        {
            MovieId = movie.Id,
            MovieTitle = movie.Title,
            MovieImageUrl = movie.ImageUrl,
            MovieDescription = movie.Description,
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRating(AddRatingViewModel model)
    {
        if(!ModelState.IsValid )
            return View(model);

        var user = await userManager.GetUserAsync(User);

        var rating = new Rating()
        {
            UserId = user.Id,
            MovieId = model.MovieId,
            StarRating = model.StarRating,
            Comment = model.Comment,
        };

        context.Ratings.Add(rating);
        await context.SaveChangesAsync();

        return RedirectToAction("Details", "Movies", new { id = model.MovieId });
    }
}