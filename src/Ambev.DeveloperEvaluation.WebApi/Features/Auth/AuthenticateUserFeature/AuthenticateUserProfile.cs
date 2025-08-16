using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// AutoMapper profile for authentication-related mappings
/// </summary>
public sealed class AuthenticateUserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticateUserProfile"/> class
    /// </summary>
    public AuthenticateUserProfile()
    {
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>(MemberList.Source)
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>(MemberList.Destination)
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
    }
}
