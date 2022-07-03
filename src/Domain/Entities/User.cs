using Domain.Enums;

namespace Domain.Entities;
public class User
{
    public User(Guid id, string name, string password, Role role)
    {
        Id = id;
        Name = name;
        Password = password;
        Role = role;
    }

    public User(string name, string password, Role role)
    {
        Name = name;
        Password = password;
        Role = role;
    }

    public User() { }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}