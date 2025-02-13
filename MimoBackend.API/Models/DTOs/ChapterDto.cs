using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DTOs;

public class ChapterDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Order { get; set; }
    public ICollection<LessonDto> Lessons { get; set; }
    public int CourseId { get; set; }
    public CourseDto Course { get; set; }
}