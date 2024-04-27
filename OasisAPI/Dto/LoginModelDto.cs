using System.ComponentModel.DataAnnotations;

namespace OasisAPI.Dto;

public class LoginModelDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }
}