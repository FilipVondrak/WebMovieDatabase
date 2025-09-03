using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMovieDatabase.Models;

public class AddMovieViewModel
{

    public Movie Movie { get; set; } = new();
    public List<int> SelectedActorIds { get; set; } = new();

    // a list of all available actors formatted for a dropdown list
    public IEnumerable<SelectListItem> AvailableActors { get; set; } = new List<SelectListItem>();
}