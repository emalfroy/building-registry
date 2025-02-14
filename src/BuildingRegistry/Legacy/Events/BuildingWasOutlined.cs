namespace BuildingRegistry.Legacy.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;

    [EventTags(EventTag.For.Sync)]
    [EventName("BuildingWasOutlined")]
    [EventDescription("Het gebouw werd ingeschetst.")]
    public class BuildingWasOutlined : IHasProvenance, ISetProvenance, IMessage
    {
        [EventPropertyDescription("Interne GUID van het gebouw.")]
        public Guid BuildingId { get; }

        [EventPropertyDescription("Extended WKB-voorstelling van de gebouwgeometrie (Hexadecimale notatie).")]
        public string ExtendedWkbGeometry { get; }

        [EventPropertyDescription("Metadata bij het event.")]
        public ProvenanceData Provenance { get; private set; }

        public BuildingWasOutlined(
            BuildingId buildingId,
            ExtendedWkbGeometry geometry)
        {
            BuildingId = buildingId;
            ExtendedWkbGeometry = geometry.ToString();
        }

        [JsonConstructor]
        private BuildingWasOutlined(
            Guid buildingId,
            string extendedWkbGeometry,
            ProvenanceData provenance)
            : this(
                new BuildingId(buildingId),
                new ExtendedWkbGeometry(extendedWkbGeometry)) => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
