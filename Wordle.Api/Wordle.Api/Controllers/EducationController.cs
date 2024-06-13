using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Wordle.Api.Dtos;
using Wordle.Api.Models;
//using Wordle.Api.Services;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Wordle.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/education
        [HttpGet("GetEducations")]
        public async Task<ActionResult<IEnumerable<Education>>> GetEducations()
        {
            return await _context.Educations.ToListAsync();
        }

        // POST: api/education
        [HttpPost("PostEducation")]
        public async Task<ActionResult<Education>> PostEducation(Education education)
        {
            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEducations), new { id = education.Id }, education);
        }

        // DELETE: api/education/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

