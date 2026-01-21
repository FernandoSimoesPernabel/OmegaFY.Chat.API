namespace OmegaFY.Chat.API.Tests.Integration.Base;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}