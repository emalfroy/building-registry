namespace BuildingRegistry.Projections.Legacy.BuildingSyndication
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using BuildingRegistry.Legacy;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;

    public class BuildingSyndicationItem
    {
        public long Position { get; set; }

        [Obsolete("Guid identifiers are no longer used.")]
        public Guid? BuildingId { get; set; }
        public int? PersistentLocalId { get; set; }
        public string? ChangeType { get; set; }

        public byte[]? Geometry { get; set; }
        public BuildingGeometryMethod? GeometryMethod { get; set; }

        public BuildingStatus? Status { get; set; }

        public bool IsComplete { get; set; }

        public DateTimeOffset RecordCreatedAtAsDateTimeOffset { get; set; }
        public DateTimeOffset LastChangedOnAsDateTimeOffset { get; set; }

        public Instant RecordCreatedAt
        {
            get => Instant.FromDateTimeOffset(RecordCreatedAtAsDateTimeOffset);
            set => RecordCreatedAtAsDateTimeOffset = value.ToDateTimeOffset();
        }

        public Instant LastChangedOn
        {
            get => Instant.FromDateTimeOffset(LastChangedOnAsDateTimeOffset);
            set => LastChangedOnAsDateTimeOffset = value.ToDateTimeOffset();
        }

        public Application? Application { get; set; }
        public Modification? Modification { get; set; }
        public string? Operator { get; set; }
        public Organisation? Organisation { get; set; }
        public string? Reason { get; set; }
        public string? EventDataAsXml { get; set; }
        public DateTimeOffset SyndicationItemCreatedAt { get; set; }

        public virtual Collection<BuildingUnitSyndicationItem> BuildingUnits { get; set; }
        public virtual Collection<BuildingUnitSyndicationItemV2> BuildingUnitsV2 { get; set; }

        public BuildingSyndicationItem()
        {
            BuildingUnits = new Collection<BuildingUnitSyndicationItem>();
            BuildingUnitsV2 = new Collection<BuildingUnitSyndicationItemV2>();
        }

        public BuildingSyndicationItem CloneAndApplyEventInfo(
            long position,
            string changeType,
            Instant lastChangedOn,
            Action<BuildingSyndicationItem> editFunc)
        {
            var buildingUnits = BuildingUnits.Select(x => x.CloneAndApplyEventInfo(position));
            var buildingUnitsV2 = BuildingUnitsV2.Select(x => x.CloneAndApplyEventInfo(position));

            var newItem = new BuildingSyndicationItem
            {
                ChangeType = changeType,
                Position = position,
                LastChangedOn = lastChangedOn,

                BuildingId = BuildingId,
                PersistentLocalId = PersistentLocalId,
                GeometryMethod = GeometryMethod,
                Geometry = Geometry,
                Status = Status,
                IsComplete = IsComplete,
                RecordCreatedAt = RecordCreatedAt,
                Application = Application,
                Modification = Modification,
                Operator = Operator,
                Organisation = Organisation,
                Reason = Reason,
                BuildingUnits = new Collection<BuildingUnitSyndicationItem>(buildingUnits.ToList()),
                BuildingUnitsV2 = new Collection<BuildingUnitSyndicationItemV2>(buildingUnitsV2.ToList()),
                SyndicationItemCreatedAt = DateTimeOffset.Now
            };

            editFunc(newItem);

            return newItem;
        }
    }

    public class BuildingSyndicationConfiguration : IEntityTypeConfiguration<BuildingSyndicationItem>
    {
        private const string TableName = "BuildingSyndication";

        public void Configure(EntityTypeBuilder<BuildingSyndicationItem> b)
        {
            b.ToTable(TableName, Schema.Legacy)
                .HasKey(x => x.Position)
                .IsClustered();

            b.Property(x => x.Position).ValueGeneratedNever();
            b.HasIndex(x => x.Position).IsColumnStore($"CI_{TableName}_Position");

            b.Property(x => x.BuildingId);
            b.Property(x => x.ChangeType);

            b.Property(x => x.Geometry);
            b.Property(x => x.GeometryMethod);

            b.Property(x => x.Status);

            b.Property(x => x.IsComplete);

            b.Property(x => x.RecordCreatedAtAsDateTimeOffset).HasColumnName("RecordCreatedAt");
            b.Property(x => x.LastChangedOnAsDateTimeOffset).HasColumnName("LastChangedOn");

            b.Property(x => x.Application);
            b.Property(x => x.Modification);
            b.Property(x => x.Operator);
            b.Property(x => x.Organisation);
            b.Property(x => x.Reason);
            b.Property(x => x.EventDataAsXml);
            b.Property(x => x.SyndicationItemCreatedAt).IsRequired();

            b.HasMany(x => x.BuildingUnits)
                .WithOne()
                .HasForeignKey(x => x.Position)
                .IsRequired();

            b.HasMany(x => x.BuildingUnitsV2)
                .WithOne()
                .HasForeignKey(x => x.Position)
                .IsRequired();

            b.Ignore(x => x.RecordCreatedAt);
            b.Ignore(x => x.LastChangedOn);

            b.HasIndex(x => x.BuildingId);
            b.HasIndex(x => x.PersistentLocalId);
        }
    }
}
