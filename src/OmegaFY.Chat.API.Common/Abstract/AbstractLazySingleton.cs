namespace OmegaFY.Chat.API.Common.Abstract;

public abstract class AbstractLazySingleton<TSingleton> where TSingleton : new()
{
    protected static readonly Lazy<TSingleton> _lazySingleton = new(() => new TSingleton());

    public static TSingleton Instance => _lazySingleton.Value;
}