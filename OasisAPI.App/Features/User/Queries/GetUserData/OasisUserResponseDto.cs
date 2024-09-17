namespace OasisAPI.App.Features.User.Queries.GetUserData;

public class OasisUserResponseDto
{
    public int OasisUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}