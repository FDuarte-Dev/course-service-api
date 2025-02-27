using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class UserAchievement
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool Completed { get; set; }
    public int Progress { get; set; }
    public string UserUsername { get; set; }
    public User User { get; set; }
    public int AchievementId { get; set; }
    public Achievement Achievement { get; set; }
}