using System.Diagnostics;
using MCAStudentProjectManagement.Data;
using Microsoft.AspNetCore.Mvc;
using MCAStudentProjectManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CollegeDbContext _db;

    public HomeController(ILogger<HomeController> logger, CollegeDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = new DashboardViewModel
        {
            TotalStudents = await _db.Students.CountAsync(),
            TotalGuides = await _db.FacultyGuides.CountAsync(),
            TotalProjects = await _db.ProjectSubmissions.CountAsync(),
            CompletedProjects = await _db.ProjectSubmissions.CountAsync(project => project.Status == "Completed"),
            RecentSubmissions = await _db.ProjectSubmissions
                .Include(project => project.Student)
                .Include(project => project.FacultyGuide)
                .OrderByDescending(project => project.SubmissionDate)
                .Take(5)
                .ToListAsync()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
