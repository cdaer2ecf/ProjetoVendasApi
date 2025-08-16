using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Represents the response returned after user authentication
/// </summary>
public sealed class AuthenticateUserResponse
{
    /// <summary>
    /// JWT Bearer token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
