using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class LessonProgress
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string StartTime { get; set; }
    public string CompletionTime { get; set; }
    public string UserUsername { get; set; }
    public User User { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
}