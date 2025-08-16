using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
//using Ambev.DeveloperEvaluation.Domain.Users;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserHandler
    : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwt;

    public AuthenticateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken ct)
    {
        // 1) Buscar por USERNAME (mandatório pela prova). Opcional: fallback por e-mail se quiser.
        var user = await _userRepository.GetByUsernameAsync(request.Username, ct);


        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 2) Validar senha: aceita texto puro OU hash (funciona nos dois cenários)
        var ok = string.Equals(user.Password, request.Password)
                 || _passwordHasher.VerifyPassword(request.Password, user.Password);
        if (!ok)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 3) Precisa estar ativo
        var activeSpec = new ActiveUserSpecification();
        if (!activeSpec.IsSatisfiedBy(user))
            throw new UnauthorizedAccessException("User is not active");

        // 4) Gerar Token
        var token = _jwt.GenerateToken(user);

        return new AuthenticateUserResult { Token = token };
    }
}
