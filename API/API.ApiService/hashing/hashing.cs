namespace API.ApiService.hashing;
public class Hashing
{

    public static string HashPassword(string haslo)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(haslo);
        byte[] hash = System.Security.Cryptography.SHA256.HashData(data);
        return Convert.ToBase64String(hash);
    }

}