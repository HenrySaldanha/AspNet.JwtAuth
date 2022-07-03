using Domain.Entities;
using Repository.Data;
using Repository.IRepository;

namespace Repository.Repository;
public class UserRepositoryWriter : IUserRepositoryWriter
{
    public User Create(User user)
    {
        UserData._users.Add(user);
        return user;
    }

    public void Delete(User user)
    {
        UserData._users.Remove(user);
    }
}