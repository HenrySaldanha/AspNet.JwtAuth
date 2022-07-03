using Domain.Entities;

namespace Application.IServices;
public interface IUserService
{
    public User Get(string name, string password);
    public IEnumerable<User> Get();
    public User Create(User user);
    public void Delete(Guid id);
}
