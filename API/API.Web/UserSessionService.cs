public class UserSessionService
{
    public bool IsLoggedIn { get; private set; }
    public string Username { get; private set; }

    public void SetUser(string username)
    {
        IsLoggedIn = true;
        Username = username;
    }

    public void Clear()
    {
        IsLoggedIn = false;
        Username = null;
    }
}