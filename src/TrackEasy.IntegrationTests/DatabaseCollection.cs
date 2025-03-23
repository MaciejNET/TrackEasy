namespace TrackEasy.IntegrationTests;

[CollectionDefinition("Sequential", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;