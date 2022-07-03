using Api.Models.Request;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("v{version:apiVersion}/[controller]")]
public class LoginController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public LoginController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] LoginRequest request)
    {
        var user = _userService.Get(request.Name, request.Password);

        if (user is null)
            return BadRequest("Invalid Name or Password");

        return Ok(_tokenService.GenerateToken(user));
    }
}