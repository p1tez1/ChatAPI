using MediatR;

namespace ChatAPI.Fetures.Auth.Login
{
    public record class LoginUserCommand(string UserName) : IRequest<IResult>;
}
