using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

[Table("oasis_users")]
public class OasisUser : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "This email is too long")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [MaxLength(300, ErrorMessage = "Password is too long")]
    public string Password { get; set; }
    
    [StringLength(300)]
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    
    [JsonIgnore]
    public ICollection<OasisChat>? Chats { get; set; }
    
    public OasisUser()
    {
        Chats = new Collection<OasisChat>();
    }
}