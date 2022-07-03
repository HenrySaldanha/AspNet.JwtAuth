using Domain.Entities;
using Domain.Enums;

namespace Api.Models.Request;
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public static implicit operator User(CreateUserRequest request)
    {
        return (request is null) ? null : new User(request.Name, request.Password, request.Role);
    }
}

