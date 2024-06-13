namespace Wordle.Api.Models;

public class Education
{
  public int Id { get; set; } // Assuming you use an auto-incrementing Id
  public string? SchoolName { get; set; }
  public string? DatesAttended { get; set; }
  public string? Accomplishment { get; set; }
}
