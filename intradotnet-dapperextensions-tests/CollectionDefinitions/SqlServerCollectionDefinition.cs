using IntraDotNet.DapperExtensions.Tests.Fixtures;

namespace IntraDotNet.DapperExtensions.Tests.CollectionDefinitions;

[CollectionDefinition("SqlServerTestCollection")]
public class SqlServerCollection : ICollectionFixture<SqlServerTestContainerFixture>
{
    // This class has no code, and is never created. Its purpose is just to be the [CollectionDefinition] anchor.
}
