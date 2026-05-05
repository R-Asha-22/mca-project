using System.ComponentModel.DataAnnotations;

namespace MCAStudentProjectManagement.Models;

public class ProjectSubmission
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Faculty Guide")]
    public int FacultyGuideId { get; set; }

    [Required, StringLength(120)]
    [Display(Name = "Project Title")]
    public string ProjectTitle { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Category { get; set; } = "Web Application";

    [Required, StringLength(30)]
    public string Status { get; set; } = "Submitted";

    [DataType(DataType.Date)]
    [Display(Name = "Submission Date")]
    public DateTime SubmissionDate { get; set; } = DateTime.Today;

    [Range(0, 100)]
    public int Marks { get; set; }

    public Student? Student { get; set; }

    public FacultyGuide? FacultyGuide { get; set; }
}
