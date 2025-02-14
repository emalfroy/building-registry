namespace BuildingRegistry.Tests.BackOffice.Api.WhenCorrectingBuildingNotRealization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using BuildingRegistry.Api.BackOffice.Abstractions.Building.Requests;
    using BuildingRegistry.Api.BackOffice.Building;
    using Building;
    using Building.Exceptions;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenBuildingIsRemovedException : BackOfficeApiTest
    {
        private readonly BuildingController _controller;

        public GivenBuildingIsRemovedException(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _controller = CreateBuildingControllerWithUser<BuildingController>();
        }

        [Fact]
        public void ThenThrowApiException()
        {
            var buildingPersistentLocalId = Fixture.Create<BuildingPersistentLocalId>();

            var request = new CorrectBuildingNotRealizationRequest
            {
                PersistentLocalId = buildingPersistentLocalId
            };

            MockMediator
                .Setup(x => x.Send(It.IsAny<CorrectBuildingNotRealizationRequest>(), CancellationToken.None).Result)
                .Throws(new BuildingIsRemovedException(buildingPersistentLocalId));

            //Act
            Func<Task> act = async () => await _controller.CorrectNotRealization(
                ResponseOptions,
                MockValidRequestValidator<CorrectBuildingNotRealizationRequest>(),
                null,
                MockIfMatchValidator(true),
                request,
                null,
                CancellationToken.None);

            // Assert
            act
                .Should()
                .ThrowAsync<ApiException>()
                .Result
                .Where(x =>
                    x.StatusCode == StatusCodes.Status410Gone
                    && x.Message == "Verwijderd gebouw.");
        }
    }
}
