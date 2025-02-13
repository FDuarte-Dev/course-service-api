using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DTOs;

public class LessonProgressDto
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string StartTime { get; set; }
    public string CompletionTime { get; set; }
    public string UserUsername { get; set; }
    public UserDto User { get; set; }
    public int LessonId { get; set; }
    public LessonDto Lesson { get; set; }
}