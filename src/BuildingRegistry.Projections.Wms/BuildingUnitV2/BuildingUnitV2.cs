namespace BuildingRegistry.Projections.Wms.BuildingUnitV2
{
    using System;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.Utilities;
    using BuildingRegistry.Building;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;

    public class BuildingUnitV2
    {
        public const string VersionTimestampBackingPropertyName = nameof(VersionTimestampAsDateTimeOffset);

        public int BuildingUnitPersistentLocalId { get; set; }
        public string Id { get; set; }

        public int BuildingPersistentLocalId { get; set; }

        public byte[]? Position { get; set; }
        public string PositionMethod { get; set; }
        public string Function { get; set; }

        public BuildingUnitStatus Status { get; set; }

        private DateTimeOffset VersionTimestampAsDateTimeOffset { get; set; }
        public Instant Version
        {
            get => Instant.FromDateTimeOffset(VersionTimestampAsDateTimeOffset);
            set
            {
                VersionTimestampAsDateTimeOffset = value.ToDateTimeOffset();
                VersionAsString = new Rfc3339SerializableDateTimeOffset(value.ToBelgianDateTimeOffset()).ToString();
            }
        }

        public string? VersionAsString { get; protected set; }
    }

    public class BuildingUnitConfiguration : IEntityTypeConfiguration<BuildingUnitV2>
    {
        public const string TableName = "BuildingUnitsV2";

        public void Configure(EntityTypeBuilder<BuildingUnitV2> b)
        {
            b.ToTable(TableName, Schema.Wms)
                .HasKey(p => p.BuildingUnitPersistentLocalId)
                .IsClustered();

            b.Property(p => p.BuildingUnitPersistentLocalId)
                .ValueGeneratedNever();

            b.Property(p => p.Id)
                .HasColumnType("varchar(53)")
                .HasMaxLength(53);

            b.Property(BuildingUnitV2.VersionTimestampBackingPropertyName)
                .HasColumnName("Version");
            b.Property(p => p.VersionAsString);

            b.Ignore(x => x.Version);

            b.Property(p => p.BuildingPersistentLocalId);
            b.Property(p => p.Function)
                .HasColumnType("varchar(21)")
                .HasMaxLength(21);

            b.Property(p => p.PositionMethod)
                .HasColumnType("varchar(22)")
                .HasMaxLength(22);

            b.Property(p => p.Position);

            b.Property(p => p.Status)
                .HasConversion(x => x.Status, y => BuildingUnitStatus.Parse(y));

            b.HasIndex(p => p.BuildingPersistentLocalId);
            b.HasIndex(p => p.Status);
        }
    }
}
