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

    public User Create(User user)
    {
        Log.Information($"Creating new user: {user.Name} {user.Role}");
        var users = _userRepositoryReader.Get();

        if(users.Any(c=>c.Name == user.Name))
        {
            Log.Information($"Username already registered");
            return null;
        }

        user.Id = Guid.NewGuid();
        return _userRepositoryWriter.Create(user);
    }

    public void Delete(Guid id)
    {
        Log.Information($"Deleting user: {id}");
        var user = _userRepositoryReader.Get(id);
        if(user != null)
            _userRepositoryWriter.Delete(user);
    }

    public User Get(string name, string password)
    {
        Log.Information($"Searching for username and password: {name}");
        return _userRepositoryReader.Get(name, password);
    }
    

    public IEnumerable<User> Get()
    {
        Log.Information($"Searching for all users");
        return _userRepositoryReader.Get();
    }
    
}