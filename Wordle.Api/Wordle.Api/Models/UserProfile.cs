namespace Wordle.Api.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CurrentPosition { get; set; }
    public string? Location { get; set; }
    public string? AboutMe { get; set; }
    public string[]? Skills { get; set; }
    public byte[]? AvatarUrl { get; set; }
}

