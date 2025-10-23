using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using System.Linq;
using System;

namespace ResumeScreeningSystem.Controllers
{
    public class ResumeScoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResumeScoreController (ApplicationDbContext context)

        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.JobPostings = _context.jobPostings.ToList();
            ViewBag.Resumes= _context.resumes.ToList();

            return View();
        }

        public IActionResult Analyze(int jobId, int resumeId)
        {
            var job = _context.jobPostings.FirstOrDefault(j=>j.Id == jobId);
            var resume = _context.resumes.FirstOrDefault(j=>j.Id == resumeId);

            if(job == null || resume == null)
            {
                TempData["Error"] = "Please select a valid job posting";
                return View();
            }

            var jobwords = job.JobDescription.ToLower().Split(' ', ' ', ' ','\n');
            var resumewords = resume.Extractedtext.ToLower().Split(' ', ',', '.', '\n');

            var matchcount = resumewords.Intersect(jobwords).Count();
            var ResumeScore = Math.Round(((double)matchcount / jobwords.Length) * 100, 2);

            var resumeScore = new Resumescore
            {
                jobPostingId = jobId,
                ResumeId = resumeId,
                Score = ResumeScore,
                AnalysisSummary = $"Resume matches {ResumeScore} % with '{job.JobTitle}' description."
            };

            _context.resumescores.Add(resumeScore);
            _context.SaveChanges();

            return View("Results", resumeScore);


        }

        
    }
}
