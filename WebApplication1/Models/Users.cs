using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebApplication1.Models;

public class Users
{
    public Users(string login, string first_name, string last_name, string password, string email)
    {
        this.login = login;
        this.first_name = first_name;
        this.last_name = last_name;
        this.password = password;
        this.email = email;
        if (this.login == "admin")
        {
            role_id = 1;
        }
        else
        {
            role_id = 2;
        }
    }

    public Users(string login, string password)
    {
        this.login = login;
        this.password = password;
    }
    
    [Key]
    public int id { get; set; }
    [Required]
    public string login { get; set; }
    
    public string first_name { get; set; }
    public string last_name { get; set; }
    
    [Required]
    public string password { get; set; }
    public string email { get; set; }
    public int role_id { get; set; }
}