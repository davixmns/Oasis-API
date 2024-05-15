using System.ComponentModel.DataAnnotations;

namespace OasisAPI.Dto;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [MinLength(1)]
    public string? Password { get; set; }
}