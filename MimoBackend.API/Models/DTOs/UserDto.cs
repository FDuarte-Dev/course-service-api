using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MimoBackend.API.Models.DTOs;

public class UserDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Username { get; set; }
    public string Password { get; set; }
}