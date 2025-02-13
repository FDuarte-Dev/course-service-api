using System.ComponentModel.DataAnnotations;

namespace MimoBackend.API.Models.DTOs;

public class UserDto
{
    [Key]
    public string Username { get; set; }
    public string Password { get; set; }
}