using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChatAPI.Fetures.Auth.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand,IResult>
    {
        private readonly IJwtTokenService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginUserCommandHandler(IJwtTokenService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var token = _jwtService.GenerateToken(request.UserName);

            return Task.FromResult(Results.Ok(new { token = token }));
        }
    }
}
