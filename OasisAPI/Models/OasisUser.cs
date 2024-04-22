using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OasisAPI.Models;

[Table("oasis_users")]
public class OasisUser
{
    [Key]
    public int OasisUserId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "This email is too long")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [StringLength(50)]
    public string? Password { get; set; }
    
    [JsonIgnore]
    public ICollection<OasisChat>? Chats { get; set; }
    
    public OasisUser()
    {
        Chats = new Collection<OasisChat>();
    }
}