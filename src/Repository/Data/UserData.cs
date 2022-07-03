using Domain.Entities;
using Domain.Enums;

namespace Repository.Data;
public static class UserData
{
    public static List<User> _users = new List<User>
    {
        new User(Guid.NewGuid(), "bigboss","qwerty123", Role.Administrator),
        new User(Guid.NewGuid(), "littleboss","abc123", Role.Manager),
        new User(Guid.NewGuid(), "worker","123456789", Role.Employee),
        new User(Guid.NewGuid(), "noob","p@ssw0rd", Role.Intern),
    };
}
