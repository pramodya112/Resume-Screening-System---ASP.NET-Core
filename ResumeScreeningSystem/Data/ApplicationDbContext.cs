using Microsoft.EntityFrameworkCore;
using ResumeScreeningSystem.Models;

namespace ResumeScreeningSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Recruiter> recruiters { get; set; } 
        public DbSet<JobPosting> jobPostings { get; set; }
        public DbSet<Resume> resumes { get; set; }

        public DbSet<Resumescore> resumescores { get; set; }

        public static implicit operator ApplicationDbContext(ApplicationBuilder v)
        {
            throw new NotImplementedException();
        }
    }
}