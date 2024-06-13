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

    [Route("api/[controller]")]
    [ApiController]
    public class ReferencesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReferencesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/references
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reference>>> GetReferences()
        {
            return await _context.References.ToListAsync();
        }

        // POST: api/references
        [HttpPost]
        public async Task<ActionResult<Reference>> PostReference(Reference reference)
        {
            _context.References.Add(reference);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReferences), new { id = reference.Id }, reference);
        }

        // DELETE: api/references/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReference(int id)
        {
            var reference = await _context.References.FindAsync(id);
            if (reference == null)
            {
                return NotFound();
            }

            _context.References.Remove(reference);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
