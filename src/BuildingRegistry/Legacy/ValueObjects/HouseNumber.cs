namespace BuildingRegistry.Legacy
{
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Newtonsoft.Json;

    public class HouseNumber : StringValueObject<HouseNumber>
    {
        public HouseNumber([JsonProperty("value")] string houseNumber) : base(houseNumber) { }
    }
}
