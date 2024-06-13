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
[Route("api/jobexperiences")]
public class JobExperiencesController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobExperiencesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/jobexperiences
    [HttpGet("GetJobExperiences")]
    public async Task<ActionResult<IEnumerable<JobExperience>>> GetJobExperiences()
    {
        return await _context.JobExperiences.ToListAsync();
    }

    // POST: api/jobexperiences
    [HttpPost("PostJobExperience")]
    public async Task<ActionResult<JobExperience>> PostJobExperience(JobExperience jobExperience)
    {
        _context.JobExperiences.Add(jobExperience);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJobExperiences), new { Id = jobExperience.Id }, jobExperience);
    }

    // DELETE: api/jobexperiences/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobExperience(int id)
    {
        var jobExperience = await _context.JobExperiences.FindAsync(id);
        if (jobExperience == null)
        {
            return NotFound();
        }

        _context.JobExperiences.Remove(jobExperience);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

