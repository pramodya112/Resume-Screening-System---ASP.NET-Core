using System.ComponentModel.DataAnnotations;

namespace ResumeScreeningSystem.Models
{
    public class Recruiter
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Recruiter Name")]
        public string RecruiterName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; } = string.Empty;

        public ICollection<JobPosting>? JobPostings { get; set; }
    }
}