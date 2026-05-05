using System.ComponentModel.DataAnnotations;

namespace MCAStudentProjectManagement.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(30)]
    [Display(Name = "Register Number")]
    public string RegisterNumber { get; set; } = string.Empty;

    [Required, StringLength(80), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Department { get; set; } = "MCA";

    [Range(1, 6)]
    public int Semester { get; set; } = 6;

    [StringLength(15)]
    public string Phone { get; set; } = string.Empty;

    public ProjectSubmission? ProjectSubmission { get; set; }
}
