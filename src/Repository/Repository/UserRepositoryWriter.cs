using Domain.Entities;
using Repository.Database;
using Repository.IRepository;

namespace Repository.Repository;
public class UserRepositoryWriter : IUserRepositoryWriter
{
    private readonly UserContext _userContext;

    public UserRepositoryWriter(UserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<User> CreateAsync(User user)
    {
        _userContext.Users.AddRange(user);
        await _userContext.SaveChangesAsync();

        return user;
    }

    public async Task DeleteAsync(User user)
    {
        _userContext.Users.Remove(user);
        await _userContext.SaveChangesAsync();
    }
}