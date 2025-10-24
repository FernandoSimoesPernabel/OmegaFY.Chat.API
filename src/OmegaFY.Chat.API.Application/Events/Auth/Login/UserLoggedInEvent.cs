namespace OmegaFY.Chat.API.Application.Events.Auth.Login;

public sealed record class UserLoggedInEvent : IEvent
{
    public Guid UserId { get; init; }

    public UserLoggedInEvent() { }

    public UserLoggedInEvent(Guid userId) => UserId = userId;
}