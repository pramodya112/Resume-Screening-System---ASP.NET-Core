namespace ResumeScreeningSystem.Models
{
    public class Resumescore
    {
        public int Id { get; set; }

        public  int ResumeId {  get; set; }

        public Resume? Resume { get; set; }

        public int JobPostingId { get; set; }

        public JobPosting? JobPosting { get; set; }

        public double Score { get; set; }

        public string AnalysisSummary { get; set; }

        //public string? JobPostingTitle { get; set; }

        //public string? RecruiterName { get; set; }


    }
}
