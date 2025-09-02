using System.ComponentModel.DataAnnotations;

namespace WebMovieDatabase.Models;

public class Actor
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = string.Empty;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}