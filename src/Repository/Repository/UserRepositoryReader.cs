using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.IRepository;

namespace Repository.Repository;
public class UserRepositoryReader : IUserRepositoryReader
{
    private readonly UserContext _userContext;

    public UserRepositoryReader(UserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<User> GetAsync(string name, string password)
    {
        return await _userContext.Users.FirstOrDefaultAsync(u => u.Password == password && u.Name == name);
    }

    public IEnumerable<User> Get()
    {
        return _userContext.Users;
    }

    public async Task<User> GetAsync(Guid id)
    {
        return await _userContext.Users.FirstOrDefaultAsync(c => c.Id == id);
    }
}