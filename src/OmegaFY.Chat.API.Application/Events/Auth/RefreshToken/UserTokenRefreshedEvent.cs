namespace OmegaFY.Chat.API.Application.Events.Auth.RefreshToken;

public sealed record class UserTokenRefreshedEvent : IEvent
{
    public Guid UserId { get; init; }

    public string OldRefreshToken { get; init; }

    public string NewRefreshToken { get; init; }

    public UserTokenRefreshedEvent() { }

    public UserTokenRefreshedEvent(Guid userId, string oldRefreshToken, string newRefreshToken)
    {
        UserId = userId;
        OldRefreshToken = oldRefreshToken;
        NewRefreshToken = newRefreshToken;
    }
}