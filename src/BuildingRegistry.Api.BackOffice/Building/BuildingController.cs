namespace BuildingRegistry.Api.BackOffice.Building
{
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.ETag;
    using BuildingRegistry.Building;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("2.0")]
    [AdvertiseApiVersions("2.0")]
    [ApiRoute("gebouwen")]
    [ApiExplorerSettings(GroupName = "gebouwen")]
    public partial class BuildingController : BuildingRegistryController
    {
        private readonly IMediator _mediator;

        public BuildingController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }
    }
}
