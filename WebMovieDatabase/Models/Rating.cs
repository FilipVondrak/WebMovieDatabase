using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebMovieDatabase.Models;

public class Rating
{
    public int Id { get; set; }

    [Required]
    [Range(1, 5)]
    public int StarRating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    [Required] public int MovieId { get; set; }
    [Required] public string UserId { get; set; }

    public IdentityUser User { get; set; }
    public Movie Movie { get; set; }
}