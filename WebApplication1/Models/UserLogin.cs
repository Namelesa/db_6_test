using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class UserLogin
{
    [Required]
    public string login { get; set; }
    [Required]
    public string password { get; set; }
}