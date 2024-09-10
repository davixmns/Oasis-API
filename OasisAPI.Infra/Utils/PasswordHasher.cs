namespace OasisAPI.Infra.Utils;

public static class PasswordHasher
{
    public static bool Verify(string text, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(text, hash);
    }

    public static string Hash(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }
}