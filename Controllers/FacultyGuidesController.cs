using MCAStudentProjectManagement.Data;
using MCAStudentProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Controllers;

public class FacultyGuidesController(CollegeDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var guides = await db.FacultyGuides
            .Include(guide => guide.ProjectSubmissions)
            .OrderBy(guide => guide.Name)
            .ToListAsync();

        return View(guides);
    }

    public IActionResult Create()
    {
        return View(new FacultyGuide());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FacultyGuide guide)
    {
        if (!ModelState.IsValid)
        {
            return View(guide);
        }

        db.FacultyGuides.Add(guide);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
