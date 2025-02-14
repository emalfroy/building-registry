namespace BuildingRegistry.Tests.AggregateTests.WhenPlanningBuilding
{
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Building;
    using Building.Commands;
    using Building.Events;
    using Fixtures;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenBuildingAlreadyExists : BuildingRegistryTest
    {
        public GivenBuildingAlreadyExists(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Customize(new WithFixedBuildingId());
            Fixture.Customize(new WithFixedBuildingPersistentLocalId());
        }

        [Fact]
        public void ThenThrowInvalidOperationException()
        {
            var buildingWasMigrated = Fixture.Create<BuildingWasMigrated>();
            
            var command = Fixture.Create<PlanBuilding>();

            Assert(new Scenario()
                .Given(
                    new BuildingStreamId(command.BuildingPersistentLocalId),
                    buildingWasMigrated)
                .When(command)
                .Throws(new AggregateSourceException($"Building with id {command.BuildingPersistentLocalId} already exists")));
        }
    }
}
