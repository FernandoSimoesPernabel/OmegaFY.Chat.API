using OmegaFY.Chat.API.Common.Abstract;

namespace OmegaFY.Chat.API.Common.Models;

public sealed class ProjectVersion : AbstractLazySingleton<ProjectVersion>
{
    public Version Version { get; }

    public ProjectVersion() => Version = System.Reflection.Assembly.GetAssembly(typeof(ProjectVersion)).GetName().Version;

    public override string ToString() => Version.ToString();
}