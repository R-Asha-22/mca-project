using System.ComponentModel.DataAnnotations;

namespace MCAStudentProjectManagement.Models;

public class FacultyGuide
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Guide Name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(80), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Specialization { get; set; } = string.Empty;

    public ICollection<ProjectSubmission> ProjectSubmissions { get; set; } = [];
}
