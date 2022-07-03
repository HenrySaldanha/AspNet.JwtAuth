using Domain.Entities;

namespace Application.IServices;
public interface IUserService
{
    public Task<User> GetAsync(string name, string password);
    public IEnumerable<User> Get();
    public Task<User> CreateAsync(User user);
    public Task DeleteAsync(Guid id);
}
