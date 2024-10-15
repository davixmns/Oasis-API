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
        if(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            throw new ArgumentException("The name, email or password of the user cannot be null or empty.");
        
        Name = name;
        Email = email;
        Password = password;
        RefreshToken = null;
        RefreshTokenExpiryDateTime = DateTime.UtcNow;
        Chats = new Collection<OasisChat>();
    }
    
    public OasisUser()
    {
        Chats = new Collection<OasisChat>();
    }

    public OasisChat AddChat(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("The title of the chat cannot be null or empty.");
        
        var newChat = new OasisChat(oasisUserId: Id, title: title);
        Chats.Add(newChat);
        return newChat;
    }
}