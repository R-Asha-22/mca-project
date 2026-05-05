using MCAStudentProjectManagement.Data;
using MCAStudentProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Controllers;

public class StudentsController(CollegeDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search)
    {
        var students = db.Students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            students = students.Where(student =>
                student.Name.Contains(search) ||
                student.RegisterNumber.Contains(search) ||
                student.Email.Contains(search));
        }

        ViewData["Search"] = search;
        return View(await students.OrderBy(student => student.Name).ToListAsync());
    }

    public IActionResult Create()
    {
        return View(new Student());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        db.Students.Add(student);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await db.Students.FindAsync(id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(student);
        }

        db.Update(student);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var student = await db.Students
            .Include(item => item.ProjectSubmission)
            .FirstOrDefaultAsync(item => item.Id == id);

        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is not null)
        {
            db.Students.Remove(student);
            await db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
