using MCAStudentProjectManagement.Data;
using MCAStudentProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Controllers;

public class ProjectsController(CollegeDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? status)
    {
        var projects = db.ProjectSubmissions
            .Include(project => project.Student)
            .Include(project => project.FacultyGuide)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            projects = projects.Where(project => project.Status == status);
        }

        ViewData["Status"] = status;
        return View(await projects.OrderByDescending(project => project.SubmissionDate).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await LoadDropDownsAsync();
        return View(new ProjectSubmission { SubmissionDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectSubmission project)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropDownsAsync(project.StudentId, project.FacultyGuideId);
            return View(project);
        }

        db.ProjectSubmissions.Add(project);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var project = await db.ProjectSubmissions.FindAsync(id);
        if (project is null)
        {
            return NotFound();
        }

        await LoadDropDownsAsync(project.StudentId, project.FacultyGuideId);
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectSubmission project)
    {
        if (id != project.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadDropDownsAsync(project.StudentId, project.FacultyGuideId);
            return View(project);
        }

        db.Update(project);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var project = await db.ProjectSubmissions
            .Include(item => item.Student)
            .Include(item => item.FacultyGuide)
            .FirstOrDefaultAsync(item => item.Id == id);

        return project is null ? NotFound() : View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await db.ProjectSubmissions.FindAsync(id);
        if (project is not null)
        {
            db.ProjectSubmissions.Remove(project);
            await db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadDropDownsAsync(int? studentId = null, int? guideId = null)
    {
        var assignedStudentIds = await db.ProjectSubmissions
            .Where(project => !studentId.HasValue || project.StudentId != studentId.Value)
            .Select(project => project.StudentId)
            .ToListAsync();

        var students = await db.Students
            .Where(student => !assignedStudentIds.Contains(student.Id))
            .OrderBy(student => student.Name)
            .ToListAsync();

        ViewBag.StudentId = new SelectList(students, "Id", "Name", studentId);
        ViewBag.FacultyGuideId = new SelectList(await db.FacultyGuides.OrderBy(guide => guide.Name).ToListAsync(), "Id", "Name", guideId);
        ViewBag.StatusList = new SelectList(new[] { "Submitted", "Reviewed", "Completed" });
        ViewBag.CategoryList = new SelectList(new[] { "Web Application", "Database Application", "Mobile App", "AI/ML", "Cloud Application" });
    }
}
