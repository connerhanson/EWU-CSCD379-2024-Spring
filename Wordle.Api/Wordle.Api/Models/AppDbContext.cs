using Microsoft.EntityFrameworkCore;

namespace Wordle.Api.Models;

public class AppDbContext : DbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<JobExperience> JobExperiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Reference> References { get; set; }
    

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }
}
