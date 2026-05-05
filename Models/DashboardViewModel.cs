namespace MCAStudentProjectManagement.Models;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }

    public int TotalGuides { get; set; }

    public int TotalProjects { get; set; }

    public int CompletedProjects { get; set; }

    public List<ProjectSubmission> RecentSubmissions { get; set; } = [];
}
