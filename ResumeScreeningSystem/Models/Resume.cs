using System.ComponentModel.DataAnnotations;

namespace ResumeScreeningSystem.Models
{
    public class Resume
    {
       public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        public string Extractedtext { get; set; } = null!;
  
        [Required]
        public int JobPostingId { get; set; }

        public JobPosting? JobPosting { get; set; }

        //public Resumescore? ResumeScore { get; set; } (single)
      
        public ICollection<Resumescore>? ResumeScoresList { get; set; } //(collection)  

    }
}
