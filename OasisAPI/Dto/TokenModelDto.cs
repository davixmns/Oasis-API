using System.ComponentModel.DataAnnotations;

namespace OasisAPI.Dto;

public class TokenModelDto
{
    [Required]
    public string? AccessToken { get; set; }
    [Required]
    public string? RefreshToken { get; set; }
}