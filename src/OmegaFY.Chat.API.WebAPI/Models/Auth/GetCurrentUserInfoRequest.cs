using OmegaFY.Chat.API.Application.Queries.Users.GetCurrentUserInfo;

namespace OmegaFY.Chat.API.WebAPI.Models.Auth;

public sealed record class GetCurrentUserInfoRequest
{
    public GetCurrentUserInfoQuery ToQuery() => new GetCurrentUserInfoQuery();
}