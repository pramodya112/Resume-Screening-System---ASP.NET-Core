namespace ResumeScreeningSystem.Models
{
    public class JobPosting
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        
        // Foreign key to Recruiter
        public int RecruiterId { get; set; }
        public Recruiter? Recruiter { get; set; } // "Recruiter?" added to make surre that there should be a rectruiter to post the job description

        public ICollection<Resume>? Resumes { get; set; } // Navigation property for related resumes
    }
}
