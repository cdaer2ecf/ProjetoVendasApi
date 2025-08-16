namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

/// <summary>
/// Represents the response after authenticating a user
/// </summary>
public sealed class AuthenticateUserResult
{
    public string Token { get; set; } = string.Empty;
}
