namespace BuildingRegistry.Legacy
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Legacy.Commands.Crab;
    using NodaTime;

    public class ReaddressingProvenanceFactory : IProvenanceFactory<Building>
    {
        private static readonly List<Type> AllowedTypes = new List<Type>
        {
            typeof(ImportReaddressingHouseNumberFromCrab),
            typeof(ImportReaddressingSubaddressFromCrab)
        };

        private static bool CanCreateFrom(Type? type) => type != null && AllowedTypes.Contains(type);

        public bool CanCreateFrom<TCommand>() => CanCreateFrom(typeof(TCommand));

        public Provenance CreateFrom(object provenanceHolder, Building aggregate)
        {
            var commandType = provenanceHolder.GetType();
            if (CanCreateFrom(commandType))
            {
                return new Provenance(
                    SystemClock.Instance.GetCurrentInstant(),
                    Application.BuildingRegistry,
                    new Reason("Gemeentelijke fusie"),
                    new Operator("BuildingRegistry"),
                    Modification.Update,
                    Organisation.Municipality);
            }

            throw new InvalidOperationException($"Cannot create provenance for {commandType?.Name}");
        }
    }
}
