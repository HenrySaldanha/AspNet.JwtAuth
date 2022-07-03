using Domain.Entities;

namespace Repository.IRepository;
public interface IUserRepositoryReader
{
    public Task<User> GetAsync(string name, string password);
    public IEnumerable<User> Get();
    public Task<User> GetAsync(Guid id);
}