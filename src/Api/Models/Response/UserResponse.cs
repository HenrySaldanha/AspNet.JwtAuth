using Domain.Entities;
using Domain.Enums;

namespace Api.Models.Response;
public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }

    public static implicit operator UserResponse(User user)
    {
        if (user is null)
            return null;

        return new UserResponse
        {
            Role = user.Role,
            Name = user.Name,
            Id = user.Id
        };
    }
}