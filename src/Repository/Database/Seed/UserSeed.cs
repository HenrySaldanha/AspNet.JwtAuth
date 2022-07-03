using Domain.Entities;
using Domain.Enums;
using Repository.Database;

namespace Repository.Seed;
public static class UserSeed
{
    public static void Seed(this UserContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User> {
                 new User(Guid.NewGuid(), "bigboss", "qwerty123", Role.Administrator),
                 new User(Guid.NewGuid(), "littleboss", "abc123", Role.Manager),
                 new User(Guid.NewGuid(), "worker", "123456789", Role.Employee),
                 new User(Guid.NewGuid(), "noob", "p@ssw0rd", Role.Intern)
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}