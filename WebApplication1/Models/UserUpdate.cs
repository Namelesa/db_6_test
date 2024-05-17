using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class UserUpdate
{
    public UserDTO user { get; set; }
    [Required]
    public string newLogin { get; set; }
    [Required]
    public string newPassword { get; set; }
}