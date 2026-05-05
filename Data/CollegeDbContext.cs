using MCAStudentProjectManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MCAStudentProjectManagement.Data;

public class CollegeDbContext(DbContextOptions<CollegeDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();

    public DbSet<FacultyGuide> FacultyGuides => Set<FacultyGuide>();

    public DbSet<ProjectSubmission> ProjectSubmissions => Set<ProjectSubmission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasIndex(student => student.RegisterNumber)
            .IsUnique();

        modelBuilder.Entity<ProjectSubmission>()
            .HasOne(project => project.Student)
            .WithOne(student => student.ProjectSubmission)
            .HasForeignKey<ProjectSubmission>(project => project.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectSubmission>()
            .HasOne(project => project.FacultyGuide)
            .WithMany(guide => guide.ProjectSubmissions)
            .HasForeignKey(project => project.FacultyGuideId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
