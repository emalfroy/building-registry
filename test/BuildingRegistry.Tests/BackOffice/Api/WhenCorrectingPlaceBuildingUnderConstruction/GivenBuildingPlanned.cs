namespace BuildingRegistry.Tests.BackOffice.Api.WhenCorrectingPlaceBuildingUnderConstruction
{
    using System.Threading;
    using System.Threading.Tasks;
    using BuildingRegistry.Api.BackOffice.Abstractions.Building.Requests;
    using BuildingRegistry.Api.BackOffice.Abstractions.Building.Responses;
    using BuildingRegistry.Api.BackOffice.Building;
    using Building;
    using FluentAssertions;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenBuildingPlanned : BackOfficeApiTest
    {
        private readonly BuildingController _controller;

        public GivenBuildingPlanned(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _controller = CreateBuildingControllerWithUser<BuildingController>();
        }

        [Fact]
        public async Task ThenShouldSucceed()
        {
            const int expectedLocation = 123;
            const string expectedHash = "123456";

            var buildingPersistentLocalId = new BuildingPersistentLocalId(expectedLocation);

            MockMediator
                .Setup(x => x.Send(It.IsAny<CorrectPlaceBuildingUnderConstructionRequest>(), CancellationToken.None).Result)
                .Returns(new ETagResponse(string.Empty, expectedHash));

            var request = new CorrectPlaceBuildingUnderConstructionRequest
            {
                PersistentLocalId = buildingPersistentLocalId
            };

            //Act
            var result = (AcceptedWithETagResult)await _controller.CorrectPlaceUnderConstruction(ResponseOptions,
                MockValidRequestValidator<CorrectPlaceBuildingUnderConstructionRequest>(),
                null,
                MockIfMatchValidator(true),
                request,
                null,
                CancellationToken.None);

            //Assert
            MockMediator.Verify(x => x.Send(It.IsAny<CorrectPlaceBuildingUnderConstructionRequest>(), CancellationToken.None), Times.Once);

            result.StatusCode.Should().Be(202);
            result.Location.Should().Be(string.Format(BuildingDetailUrl, expectedLocation));
            result.ETag.Should().Be(expectedHash);
        }
    }
}
