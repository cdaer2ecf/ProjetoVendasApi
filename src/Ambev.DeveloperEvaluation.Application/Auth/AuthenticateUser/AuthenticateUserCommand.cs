using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

/// <summary>
/// Command for authenticating a user in the system.
/// Implements IRequest for mediator pattern handling.
/// </summary>
public sealed class AuthenticateUserCommand : IRequest<AuthenticateUserResult>
{
    /// <summary>
    /// Username do usuário (identificador primário para login).
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Senha em texto puro enviada no login.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
