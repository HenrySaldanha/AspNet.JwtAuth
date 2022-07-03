using Domain.Entities;

namespace Repository.IRepository;
public interface IUserRepositoryWriter
{
    public Task<User> CreateAsync(User user);
    public Task DeleteAsync(User user);
}