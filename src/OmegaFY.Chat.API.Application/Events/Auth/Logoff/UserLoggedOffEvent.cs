namespace OmegaFY.Chat.API.Application.Events.Auth.Logoff;

public sealed record class UserLoggedOffEvent : IEvent
{
    public Guid UserId { get; init; }

    public string CurrentRefreshToken { get; init; }

    public UserLoggedOffEvent() { }

    public UserLoggedOffEvent(Guid userId, string currentRefreshToken)
    {
        UserId = userId;
        CurrentRefreshToken = currentRefreshToken;
    }
}