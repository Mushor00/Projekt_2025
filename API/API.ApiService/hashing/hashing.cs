namespace API.ApiService.hashing;
public class Hashing
{

    public static string HashPassword(string haslo)
    {
        //to ja juz zrobiłem, todo: podrasować to jeszcze, żeby zwracało dobrze zaszyfrowane hasło
        byte[] data = System.Text.Encoding.ASCII.GetBytes(haslo);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        haslo = System.Text.Encoding.ASCII.GetString(data);
        return haslo;
    }

}