using Domain.Entities;

namespace Repository.IRepository;
public interface IUserRepositoryWriter
{
    public User Create(User user);
    public void Delete(User user);
}