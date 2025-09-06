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

    /// <summary>
    /// AddRating get function which checks and fetches the movie from db
    /// </summary>
    /// <param name="id">id of the movie to be rated</param>
    /// <returns>the view with movie details, if id is invalid then redirects to error page</returns>
    public async Task<IActionResult> AddRating(int id)
    {
        var movie = await context.Movies.FindAsync(id);

        if (movie == null)
            return RedirectToAction("InvalidId",  "Error");

        var model = new AddRatingViewModel()
        {
            MovieId = movie.Id,
            MovieTitle = movie.Title,
            MovieImageUrl = movie.ImageUrl,
            MovieDescription = movie.Description,
        };
        return View(model);
    }

    /// <summary>
    /// AddRating post function which takes the inputted user data and stores them in the db with the associated movie
    /// </summary>
    /// <param name="model">
    /// model received from page with the user inputted rating
    /// </param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRating(AddRatingViewModel model)
    {
        if(!ModelState.IsValid )
            return View(model);

        var user = await userManager.GetUserAsync(User);

        if(user == null)
            return View(model);

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