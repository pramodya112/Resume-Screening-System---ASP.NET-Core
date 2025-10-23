namespace ResumeScreeningSystem.Models
{
    public class Recruiter
    {
        public int Id { get; set; }
        public string RecruiterName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public ICollection <JobPosting>?  JobPostings { get; set; } 


    }
}
