namespace BuildingRegistry.Tests.AggregateTests.SnapshotTests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Snapshotting;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Building;
    using Building.Commands;
    using Building.Events;
    using Fixtures;
    using FluentAssertions;
    using Legacy.Autofixture;
    using Newtonsoft.Json;
    using WhenPlanningBuildingUnit;
    using Xunit;
    using Xunit.Abstractions;
    using BuildingUnit = Building.BuildingUnit;

    public class GivenBuilding : BuildingRegistryTest
    {
        private readonly BuildingStreamId _streamId;

        public GivenBuilding(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Customize(new InfrastructureCustomization());
            Fixture.Customize(new WithFixedBuildingPersistentLocalId());
            Fixture.Customize(new WithValidPolygon());
            _streamId = new BuildingStreamId(Fixture.Create<BuildingPersistentLocalId>());
        }

        [Fact]
        public async Task ThenSnapshotWasCreated()
        {
            Fixture.Register(() => (ISnapshotStrategy)IntervalStrategy.SnapshotEvery(1));

            var provenance = Fixture.Create<Provenance>();
            var buildingWasPlanned = Fixture.Create<BuildingWasPlannedV2>();

            var buildingUnitWasPlanned = Fixture.Create<BuildingUnitWasPlannedV2>();

            var buildingGeometry = new BuildingGeometry(new ExtendedWkbGeometry(buildingWasPlanned.ExtendedWkbGeometry),
                BuildingGeometryMethod.Outlined);

            var planBuildingUnit = Fixture.Create<PlanBuildingUnit>()
                .WithoutPosition()
                .WithProvenance(provenance);

            var expectedEvent = new BuildingUnitWasPlannedV2(
                planBuildingUnit.BuildingPersistentLocalId,
                planBuildingUnit.BuildingUnitPersistentLocalId,
                planBuildingUnit.PositionGeometryMethod,
                buildingGeometry.Center,
                BuildingUnitFunction.Unknown,
                planBuildingUnit.HasDeviation);
            ((ISetProvenance)expectedEvent).SetProvenance(provenance);

            var expectedEvent2 = new CommonBuildingUnitWasAddedV2(
                planBuildingUnit.BuildingPersistentLocalId,
                new BuildingUnitPersistentLocalId(1),
                BuildingUnitStatus.Planned, 
                BuildingUnitPositionGeometryMethod.DerivedFromObject, 
                buildingGeometry.Center,
                hasDeviation: false);
            ((ISetProvenance)expectedEvent2).SetProvenance(provenance);

            var plannedBuildingUnit = new BuildingUnit(o => { });
            plannedBuildingUnit.Route(buildingUnitWasPlanned);
            var buildingUnit = new BuildingUnit(o => { });
            buildingUnit.Route(expectedEvent);
            var commonBuildingUnit = new BuildingUnit(o => { });
            commonBuildingUnit.Route(expectedEvent2);

            var expectedSnapshot = new BuildingSnapshot(
                Fixture.Create<BuildingPersistentLocalId>(),
                BuildingStatus.Planned,
                buildingGeometry,
                false,
                expectedEvent2.GetHash(),
                expectedEvent2.Provenance,
                new List<BuildingUnit>
                {
                    plannedBuildingUnit,
                    buildingUnit,
                    commonBuildingUnit
                });

            Assert(new Scenario()
                .Given(_streamId,
                    buildingWasPlanned,
                    buildingUnitWasPlanned)
                .When(planBuildingUnit)
                .Then(
                    new Fact(_streamId, expectedEvent),
                    new Fact(_streamId, expectedEvent2)));

            var snapshotStore = (ISnapshotStore)Container.Resolve(typeof(ISnapshotStore));
            var latestSnapshot = await snapshotStore.FindLatestSnapshotAsync(_streamId, CancellationToken.None);

            latestSnapshot.Should().NotBeNull();
            latestSnapshot
                .Should()
                .BeEquivalentTo(
                    Build(
                        expectedSnapshot,
                        3,
                        EventSerializerSettings));
        }

        private static SnapshotContainer Build(
            BuildingSnapshot snapshot,
            long streamVersion,
            JsonSerializerSettings serializerSettings)
        {
            return new SnapshotContainer
            {
                Info = new SnapshotInfo { StreamVersion = streamVersion, Type = nameof(BuildingSnapshot) },
                Data = JsonConvert.SerializeObject(snapshot, serializerSettings)
            };
        }
    }
}
