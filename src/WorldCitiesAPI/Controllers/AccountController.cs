using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WorldCitiesAPI.Controllers;

[AllowAnonymous]
[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
    {
        _context = context;
        _signInManager = signInManager;
        _logger = logger;
    }

    private ActionResult Error()
    {
        _logger.LogError("Invalid login attempt.");

        return Unauthorized(new LoginResult
        {
            Message = "Invalid Email or Password."
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _signInManager.UserManager.FindByNameAsync(loginRequest.Email);

        if (user == null)
        {
            return Error();
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);

        if (result == SignInResult.Success)
        {
            _logger.LogInformation("User {@User} logged in.", user);

            return Ok(new LoginResult { Success = true });
        }

        return Error();
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        var user = await _signInManager.UserManager.GetUserAsync(User);

        if (user == null)
        {
            return NoContent();
        }

        await _signInManager.SignOutAsync();

        _logger.LogInformation("User {@User} logged out.", user);

        return NoContent();
    }
}
