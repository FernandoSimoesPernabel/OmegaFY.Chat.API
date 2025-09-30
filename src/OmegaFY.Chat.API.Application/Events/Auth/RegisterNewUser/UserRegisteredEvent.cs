
namespace OmegaFY.Chat.API.Application.Events.Auth.RegisterNewUser;

public sealed record class UserRegisteredEvent : IEvent
{
    public Guid UserId { get; set; }

    public string Email { get; init; }

    public string DisplayName { get; init; }

    public UserRegisteredEvent(Guid userId, string email, string displayName)
    {
        UserId = userId;
        Email = email;
        DisplayName = displayName;
    }
}