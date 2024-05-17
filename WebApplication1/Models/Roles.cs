using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Roles
{
    [Key]
    public int id { get; set; }

    public string alias { get; set; }

}