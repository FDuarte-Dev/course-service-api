using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class UserAchievements
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool Completed { get; set; }
    public int Progress { get; set; }
}