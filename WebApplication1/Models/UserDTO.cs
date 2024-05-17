using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class UserDTO
{
    [Required]
    public string login { get; set; }
    [Required]
    public string first_name { get; set; }
    [Required]
    public string last_name { get; set; }
    
    [Required]
    public string password { get; set; }
    [Required]
    public string email { get; set; }
}