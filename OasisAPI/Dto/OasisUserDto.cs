namespace OasisAPI.Dto;

using System.ComponentModel.DataAnnotations;

public class OasisUserDto
{
    public int OasisUserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
