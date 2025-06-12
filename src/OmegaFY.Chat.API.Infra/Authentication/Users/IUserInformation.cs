namespace OmegaFY.Chat.API.Infra.Authentication.Users;

public interface IUserInformation
{
    public Guid? CurrentRequestUserId { get; }

    public string Email { get; }
}