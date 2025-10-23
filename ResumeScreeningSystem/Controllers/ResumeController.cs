using Microsoft.AspNetCore.Mvc;
using ResumeScreeningSystem.Data;
using ResumeScreeningSystem.Models;
using ResumeScreeningSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection.Metadata.Ecma335;


namespace ResumeScreeningSystem.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;


        public ResumeController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(int? jobPostingId)
        {
           IQueryable<Resume> resumesQuery = _context.resumes
           .Include(r => r.JobPosting)
            .ThenInclude(jp => jp.Recruiter);
        

        if (jobPostingId.HasValue)
        {
            resumesQuery = resumesQuery.Where(r => r.JobPostingId == jobPostingId.Value);
            var jobPosting = _context.jobPostings.Find(jobPostingId.Value);
            ViewBag.JobPostingTitle = jobPosting?. JobTitle;

        }

        var resumes= resumesQuery.OrderByDescending(r => r.Id).ToList();
        ViewBag.JobPostings = _context.jobPostings.ToList();

            return View (resumes);
        }
        public IActionResult Upload (int jobPostingId)
        {
            var jobposting = _context.jobPostings.Find(jobPostingId);
            if(jobposting == null)
            {
                TempData["Error"] = "Job posting not found";
                return RedirectToAction("Index", "JobPosting");
            }
            ViewBag.JobPostingId = jobPostingId;
            ViewBag.JobTitle = jobposting.JobTitle;
            return View();
        }

        [HttpPost]
        public IActionResult Upload(int jobPostingId, IFormFile resumeFile)
        {

            var jobposting = _context.jobPostings.Find(jobPostingId);
            if (jobposting == null)
            {
                TempData["Error"] = "Job posting not found.";
                return RedirectToAction("Index", "JobPosting");
            }

            if (resumeFile==null || resumeFile.Length == 0)
           {
                TempData["Error"] = "Please select a valid resume file.";
                ViewBag.JobPostingId = jobPostingId;
                ViewBag.JobTitle = jobposting.JobTitle;
                return View ();
            }

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var filePath = Path.Combine(uploads, resumeFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                resumeFile.CopyTo(fileStream);
            }

            var etractedText = ResumeParser.ExtractText(filePath);

            var resume = new Resume
            {
                FileName = resumeFile.FileName,
                FilePath = filePath,
                Extractedtext = etractedText,
                JobPostingId = jobPostingId
            };
            try
            {
                _context.resumes.Add(resume);
                _context.SaveChanges();
                TempData["Success"] = "Resume uploaded successfully.";
            }

            catch(Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine("Error saving resume" +ex.InnerException?.Message);
                TempData["Error"] = "An error occurred while saving the resume. Please try again." + ex.InnerException?.Message;
                ViewBag.JobPostingId = jobPostingId;
                ViewBag.JobTitle = jobposting.JobTitle;
                return View();
            }

            
            return RedirectToAction("Index", new { jobPostingId = jobPostingId });
        }
        public IActionResult Details(int id)
        {
            var resume = _context.resumes
                .Include (r => r.JobPosting)
                .ThenInclude (jp => jp.Recruiter)
                .FirstOrDefault (r => r.Id == id);
            if (resume == null)
            {
                TempData["Error"] = "Resume not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(resume);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var resume = _context.resumes.Find(id);
            if (resume == null)
            {
                TempData["Error"] = "Resume not found.";
                return RedirectToAction(nameof(Index));
            }
            if (System.IO.File.Exists(resume.FilePath))
            {
                try
                {
                    System.IO.File.Delete(resume.FilePath);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error deleting file: " + ex.Message);

                }
            }
                _context.resumes.Remove(resume);
                _context.SaveChanges();
                TempData["Success"] = "Resume deleted successfully.";

                return RedirectToAction(nameof(Index));
            }


        

        public IActionResult Download(int id)
        {
            var resume = _context.resumes.Find(id);
            if (resume == null)
            {
                TempData["Error"] = "Resume not found.";
                return RedirectToAction(nameof(Index));
            }
            var ifileBytes = System.IO.File.ReadAllBytes(resume.FilePath);
            var contentType = GetContentType(resume.FileName);
            return File(ifileBytes, contentType, resume.FileName);
        }

        private string GetContentType (string fileName)
        {
            var extension = Path.GetExtension (fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream",
            };
        }
    }

}
