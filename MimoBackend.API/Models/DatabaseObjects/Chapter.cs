using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class Chapter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Order { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
}