using Domain.Entities;

namespace Application.IServices;
public interface ITokenService
{
    public string GenerateToken(User user);
}
