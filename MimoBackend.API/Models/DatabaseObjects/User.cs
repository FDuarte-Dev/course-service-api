using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DatabaseObjects;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Username { get; set; }
    public string Password { get; set; }
}

public sealed class NotFoundUser : User, NotFoundObject
{
    private NotFoundUser() { }
    private static readonly NotFoundUser User = new();
    public static NotFoundUser GetNotFoundUser()
    {
        return User;
    }
}