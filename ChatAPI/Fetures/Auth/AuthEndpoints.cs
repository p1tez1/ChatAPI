using ChatAPI.Fetures.Auth.Login;
using MediatR;

namespace ChatAPI.Fetures.Auth
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/auth").WithTags("Auth");

            group.MapPost("/login", async (LoginUserCommand cmd, ISender sender) =>
            await sender.Send(cmd));
        }
    }
}
