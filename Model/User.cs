namespace Model;

internal abstract class User
{
    private readonly string _password;

    public User(string name, string username, string password)
    {
        Name = name;
        Username = username;
        _password = password;
    }

    public string Name { get; set; }
    public string Username { get; set; }

    public bool Login(string password)
    {
        return _password == password;
    }
}