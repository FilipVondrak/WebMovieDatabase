using System.ComponentModel.DataAnnotations;

namespace WebMovieDatabase.Models;

public class Movie
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
}