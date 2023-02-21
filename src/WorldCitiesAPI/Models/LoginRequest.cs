using System.ComponentModel.DataAnnotations;

namespace WorldCitiesAPI.Models;

public class LoginRequest
{
    /// <summary>
    /// Email of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
