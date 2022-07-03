using Application.IServices;
using Domain.Entities;
using Repository.IRepository;
using Serilog;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepositoryReader _userRepositoryReader;
    private readonly IUserRepositoryWriter _userRepositoryWriter;

    public UserService(IUserRepositoryReader userRepositoryReader, IUserRepositoryWriter userRepositoryWriter)
    {
        _userRepositoryReader = userRepositoryReader;
        _userRepositoryWriter = userRepositoryWriter;
    }

    public async Task<User> CreateAsync(User user)
    {
        Log.Information($"Creating new user: {user.Name} {user.Role}");
        var users = _userRepositoryReader.Get();

        if(users.Any(c=>c.Name == user.Name))
        {
            Log.Information($"Username already registered");
            return null;
        }

        user.Id = Guid.NewGuid();
        return await _userRepositoryWriter.CreateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        Log.Information($"Deleting user: {id}");
        var user = await _userRepositoryReader.GetAsync(id);
        if(user != null)
            await _userRepositoryWriter.DeleteAsync(user);
    }

    public async Task<User> GetAsync(string name, string password)
    {
        Log.Information($"Searching for username and password: {name}");
        return await _userRepositoryReader.GetAsync(name, password);
    }
    

    public IEnumerable<User> Get()
    {
        Log.Information($"Searching for all users");
        return _userRepositoryReader.Get();
    }
    
}