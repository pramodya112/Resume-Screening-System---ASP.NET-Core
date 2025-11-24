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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<JobPosting>().ToTable("jobPostings");
            modelBuilder.Entity<Resume>().ToTable("resumes");
            modelBuilder.Entity<Resumescore>().ToTable("resumescore");

            //Resume to JobPosting relationship (Many-to-One)    ("The Fluent API- To define the rules and relationships between your tables that standard C# properties might miss.)
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.JobPosting)
                .WithMany(j => j.Resumes)
                .HasForeignKey(r => r.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict); //you cant deleta a job posting if there are resumes associated with it

            //ResumeScore to Resume relationship (Many-to-One)
            modelBuilder.Entity<Resumescore>()
               .HasOne(rs => rs.Resume)
               .WithMany(r => r.ResumeScoresList)
               .HasForeignKey(rs => rs.ResumeId)
               .OnDelete(DeleteBehavior.Cascade); //when reusme is deleted, delet its scores

            //ResumeScore to JobPosting relationship (Many-to-One)
            modelBuilder.Entity<Resumescore>()
                .HasOne(rs => rs.JobPosting)
                .WithMany(jp => jp.ResumeScoresList)
                .HasForeignKey(rs => rs.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict); //or Cascade based on your requirement


        }
    }
} 