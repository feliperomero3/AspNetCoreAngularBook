namespace WorldCitiesAPI.Models;

public class LoginResult
{
    /// <summary>
    /// <see langword="true"/> if the login attempt is successful, <see langword="false"/> otherwise.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Login attempt result message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
