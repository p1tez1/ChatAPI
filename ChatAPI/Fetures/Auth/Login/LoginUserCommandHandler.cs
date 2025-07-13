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

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Task.FromResult(Results.Ok(token));
        }
    }
}
