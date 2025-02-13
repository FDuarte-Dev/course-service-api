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