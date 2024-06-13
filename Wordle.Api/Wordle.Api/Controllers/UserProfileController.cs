using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Wordle.Api.Dtos;
using Wordle.Api.Models;
//using Wordle.Api.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Wordle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserProfileController(AppDbContext context)
    {
        _context = context;
    }

    /*
    [HttpPost("Profile")]
    public async Task<IActionResult> PostUserProfile([FromForm] UserProfileWithImage userProfileWithImage)
    {
        if (userProfileWithImage.ImageFile == null || userProfileWithImage.ImageFile.Length == 0)
        {
            return BadRequest("Image file is required.");
        }

        byte[] imageData;

        using (var memoryStream = new MemoryStream())
        {
            await userProfileWithImage.ImageFile.CopyToAsync(memoryStream);
            imageData = memoryStream.ToArray();
        }

        var userProfile = new UserProfile
        {
            Name = userProfileWithImage.Name,
            CurrentPosition = userProfileWithImage.CurrentPosition,
            Location = userProfileWithImage.Location,
            AboutMe = userProfileWithImage.AboutMe,
            Skills = userProfileWithImage.Skills,
            AvatarUrl = imageData
        };

        _context.UserProfiles.Add(userProfile);
        await _context.SaveChangesAsync();

        return Ok(userProfile);
    }
*/


[HttpPost("Profile")]
public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfileWithImage userProfileWithImage)
{
    // Check if a profile already exists based on some identifier (e.g., user ID)
    var existingProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == userProfileWithImage.Id);

    byte[]? imageData = null;

    if (userProfileWithImage.ImageFile != null && userProfileWithImage.ImageFile.Length > 0)
    {
        using (var memoryStream = new MemoryStream())
        {
            await userProfileWithImage.ImageFile.CopyToAsync(memoryStream);
            imageData = memoryStream.ToArray();
        }
    }

    if (existingProfile != null)
    {
        // Update existing profile with new data
        existingProfile.Name = userProfileWithImage.Name;
        existingProfile.CurrentPosition = userProfileWithImage.CurrentPosition;
        existingProfile.Location = userProfileWithImage.Location;
        existingProfile.AboutMe = userProfileWithImage.AboutMe;
        existingProfile.Skills = userProfileWithImage.Skills;
        existingProfile.AvatarUrl = imageData; // Assuming AvatarImage is a byte[] property

        _context.UserProfiles.Update(existingProfile); // Mark entity as modified

        await _context.SaveChangesAsync();

        return Ok(existingProfile); // Return updated profile
    }
    else
    {
        // Create a new profile if not found (optional)
        var newProfile = new UserProfile
        {
            Id = userProfileWithImage.Id ?? 0, // Handle nullable int to int conversion
            Name = userProfileWithImage.Name,
            CurrentPosition = userProfileWithImage.CurrentPosition,
            Location = userProfileWithImage.Location,
            AboutMe = userProfileWithImage.AboutMe,
            Skills = userProfileWithImage.Skills,
            AvatarUrl = imageData
        };

        _context.UserProfiles.Add(newProfile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserProfile), new { id = newProfile.Id }, newProfile);
    }
}


[HttpGet("{id}")]
public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
{
    var userProfile = await _context.UserProfiles.FindAsync(id);

    if (userProfile == null)
    {
        return NotFound();
    }

    return Ok(userProfile);
}



    public class UserProfileWithImage
    {
        public int? Id { get; set; }  
        public string? Name { get; set; }
        public string? CurrentPosition { get; set; }
        public string? Location { get; set; }
        public string? AboutMe { get; set; }
        public string[]? Skills { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    // Implement other CRUD actions as needed (GET, PUT, DELETE)
}

