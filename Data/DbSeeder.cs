using MCAStudentProjectManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<CollegeDbContext>();

        await db.Database.EnsureCreatedAsync();

        if (await db.Students.AnyAsync())
        {
            return;
        }

        var students = new List<Student>
        {
            new() { Name = "Asha R", RegisterNumber = "MCA2026001", Email = "asha@example.com", Phone = "9876543210" },
            new() { Name = "Kavin S", RegisterNumber = "MCA2026002", Email = "kavin@example.com", Phone = "9876543211" },
            new() { Name = "Nithya P", RegisterNumber = "MCA2026003", Email = "nithya@example.com", Phone = "9876543212" }
        };

        var guides = new List<FacultyGuide>
        {
            new() { Name = "Dr. Priya Kumar", Email = "priya@college.edu", Specialization = "Web Technologies" },
            new() { Name = "Prof. Arjun Raj", Email = "arjun@college.edu", Specialization = "Data Mining" },
            new() { Name = "Dr. Meena Devi", Email = "meena@college.edu", Specialization = "Cloud Computing" }
        };

        db.Students.AddRange(students);
        db.FacultyGuides.AddRange(guides);
        await db.SaveChangesAsync();

        db.ProjectSubmissions.AddRange(
            new ProjectSubmission
            {
                StudentId = students[0].Id,
                FacultyGuideId = guides[0].Id,
                ProjectTitle = "Student Project Management System",
                Category = "Web Application",
                Status = "Completed",
                SubmissionDate = DateTime.Today.AddDays(-8),
                Marks = 92
            },
            new ProjectSubmission
            {
                StudentId = students[1].Id,
                FacultyGuideId = guides[1].Id,
                ProjectTitle = "Online Placement Portal",
                Category = "Database Application",
                Status = "Reviewed",
                SubmissionDate = DateTime.Today.AddDays(-4),
                Marks = 86
            });

        await db.SaveChangesAsync();
    }
}
