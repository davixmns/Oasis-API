using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OasisAPI.Models;

public class OasisUser
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    
    [Required]
    [StringLength(100)]
    public string? Email { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Password { get; set; }
    
    [JsonIgnore]
    public ICollection<OasisChat>? Chats { get; set; }
    
    public OasisUser()
    {
        Chats = new Collection<OasisChat>();
    }
}