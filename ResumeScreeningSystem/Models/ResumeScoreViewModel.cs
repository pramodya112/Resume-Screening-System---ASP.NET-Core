namespace ResumeScreeningSystem.Models
{
    public class ResumeScoreViewModel
    {

        public int ResumeId { get; set; }

        public string FileName { get; set; } =string.Empty;

        public double Score { get; set; }

        public string AnalysisSummary { get; set; } =string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string? JobPostingTitle { get; set; }

        public string? RecruiterName { get; set; }

    }
}
