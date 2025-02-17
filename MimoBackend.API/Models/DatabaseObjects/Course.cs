using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class Course
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public ICollection<Chapter> Chapters { get; set; }
}

public sealed class NotFoundCourse : Course, NotFoundObject
{
    private NotFoundCourse() { }
    private static readonly NotFoundCourse Course = new();
    public static NotFoundCourse GetNotFoundCourse()
    {
        return Course;
    }
}