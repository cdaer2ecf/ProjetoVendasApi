using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Represents the authentication request model for user login.
/// </summary>
public sealed class AuthenticateUserRequest
{
    /// <summary>
    /// Username utilizado para autenticação. (Contrato oficial: { "username", "password" })
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário (texto puro no request; será verificada contra o hash armazenado).
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}