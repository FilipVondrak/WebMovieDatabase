using System.ComponentModel.DataAnnotations;

namespace WebMovieDatabase.Models;

public class AddRatingViewModel
{
    [Required] public int MovieId { get; set; } = new();
    public string MovieTitle { get; set; } = string.Empty;
    public string? MovieImageUrl { get; set; } = string.Empty;
    public string? MovieDescription { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int StarRating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }
}