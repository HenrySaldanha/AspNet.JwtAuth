using Api.Models.Request;
using Api.Models.Response;
using Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Produces("application/json")]
[Route("v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Authorize(Roles = "Manager,Administrator")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateAsync(request);

        if (user is null)
            return BadRequest("User already registered");

        return Created(nameof(CreateUserAsync),(UserResponse)user);
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Administrator,Employee")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult GetAllUsers()
    {
        var users = _userService.Get();

        if (users is null || !users.Any())
            return NoContent();

        return Ok(users.Select(c => (UserResponse)c));
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUserAsync([FromQuery] Guid id)
    {
        await _userService.DeleteAsync(id);

        return Ok();
    }
}