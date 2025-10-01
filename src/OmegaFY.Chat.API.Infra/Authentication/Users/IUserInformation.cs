namespace OmegaFY.Chat.API.Infra.Authentication.Users;

public interface IUserInformation
{
    public bool IsAuthenticated { get; }

    public Guid? CurrentRequestUserId { get; }

    public string Email { get; }
}