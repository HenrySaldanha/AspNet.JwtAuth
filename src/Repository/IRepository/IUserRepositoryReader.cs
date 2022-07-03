using Domain.Entities;

namespace Repository.IRepository;
public interface IUserRepositoryReader
{
    public User Get(string name, string password);
    public IEnumerable<User> Get();
    public User Get(Guid id);
}