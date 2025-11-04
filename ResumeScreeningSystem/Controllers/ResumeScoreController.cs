using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Reflection.Metadata.Ecma335;

namespace ResumeScreeningSystem.Controllers
{
    public class ResumeScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResumeScoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.JobPostings = _context.jobPostings.ToList();
            ViewBag.Resumes = _context.resumes.ToList();

            return View();
        }

        [HttpGet]
        public IActionResult AnalyzeAllGet()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AnalyzeAll(int jobId)
        {
            var job = _context.jobPostings.FirstOrDefault(j => j.Id == jobId);

            if (job == null)
            {
                TempData["Error"] = "Please select a valid job posting";
                return RedirectToAction("Index");
            }

            // Get only resumes associated with this job posting
            var allResumes = _context.resumes
                .Where(r => r.JobPostingId == jobId)
                .ToList();

            if (!allResumes.Any())
            {
                TempData["Error"] = $"No resumes found for {job.JobTitle}";
                return RedirectToAction("Index");
            }

            var jobwords = job.JobDescription
                .ToLower()
                .Split(new char[] { ' ', ',', '.', '\n', '\r', '\t', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                ?? Array.Empty<string>();

            var jobWordSet = new HashSet<string>(jobwords);

            // Delete existing scores for this job to avoid duplicates
            var existingScores = _context.resumescores.Where(rs => rs.JobPostingId == jobId);
            _context.resumescores.RemoveRange(existingScores);

            // Analyze each resume
            foreach (var resume in allResumes)
            {
                var resumewords = resume.Extractedtext
                    .ToLower()
                    .Split(new char[] { ' ', ',', '.', '\n', '\r', '\t', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                    ?? Array.Empty<string>();

                var matchcount = resumewords.Count(w => jobWordSet.Contains(w));
                var score = jobWordSet.Count > 0 ? (double)matchcount / jobWordSet.Count * 100 : 0;

                var resumeScore = new Resumescore
                {
                    JobPostingId = jobId,
                    ResumeId = resume.Id,
                    Score = score,
                    AnalysisSummary = $"Resume matches {score:F1}% with '{job.JobTitle}' description."
                };

                _context.resumescores.Add(resumeScore);
            }

            _context.SaveChanges();

            TempData["Info"] = $"Successfully analyzed {allResumes.Count} resume(s) for {job.JobTitle}";
            return RedirectToAction("Results", new { jobId });
        }

        public IActionResult Results(int jobId)
        {
            var job = _context.jobPostings
                .Include(j => j.Recruiter)
                .FirstOrDefault(j => j.Id == jobId);

            if (job == null)
            {
                TempData["Error"] = "Job posting not found.";
                return RedirectToAction("Index");
            }

            ViewBag.JobTitle = job.JobTitle;

            var results = _context.resumescores
                .Include(rs => rs.Resume)
                .Include(rs => rs.JobPosting)
                .ThenInclude(jp => jp.Recruiter)
                .Where(rs => rs.JobPostingId == jobId)
                .Select(rs => new ResumeScoreViewModel
                {
                    ResumeId = rs.Resume.Id,
                    FileName = rs.Resume.FileName,
                    Score = rs.Score,
                    AnalysisSummary = rs.AnalysisSummary ?? "No summary",
                    JobTitle = rs.JobPosting.JobTitle,
                    JobPostingTitle = rs.JobPosting.JobTitle,
                    RecruiterName = rs.JobPosting.Recruiter != null
                                    ? rs.JobPosting.Recruiter.RecruiterName
                                    : "Not Assigned"
                })
                .OrderByDescending(rs => rs.Score)
                .ToList();

            return View(results);
        }
    }
}