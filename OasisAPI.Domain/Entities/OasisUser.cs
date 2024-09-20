using System.Collections.ObjectModel;

namespace Domain.Entities;

public class OasisUser : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDateTime { get; set; }
    public ICollection<OasisChat> Chats { get; set; }
    
    public OasisUser(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
        RefreshToken = null;
        RefreshTokenExpiryDateTime = DateTime.Now;
        Chats = new Collection<OasisChat>();
    }
    
    public OasisUser()
    {
        Chats = new Collection<OasisChat>();
    }
}