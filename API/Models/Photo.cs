using System.Text.Json.Serialization;

namespace API.Models;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    // how to get a FK entity relationship
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}