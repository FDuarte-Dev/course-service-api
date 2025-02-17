using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class Lesson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Order { get; set; }
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; }
}

public sealed class NotFoundLesson : Lesson, NotFoundObject
{
    private NotFoundLesson() { }
    private static readonly NotFoundLesson Lesson = new();
    public static NotFoundLesson GetNotFoundLesson()
    {
        return Lesson;
    }
}