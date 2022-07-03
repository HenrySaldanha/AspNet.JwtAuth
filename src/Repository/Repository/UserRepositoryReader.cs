using Domain.Entities;
using Repository.Data;
using Repository.IRepository;

namespace Repository.Repository;
public class UserRepositoryReader : IUserRepositoryReader
{
    public User Get(string name, string password)
    {
        return UserData._users.FirstOrDefault(u => u.Password == password && u.Name == name);
    }

    public IEnumerable<User> Get()
    {
        return UserData._users;
    }

    public User Get(Guid id)
    {
        return UserData._users.FirstOrDefault(c=>c.Id == id);
    }
}