namespace BuildingRegistry.Projections.Extract.BuildingExtract
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BuildingExtractItemV2
    {
        public int PersistentLocalId { get; set; }
        public byte[] DbaseRecord { get; set; }
        public byte[]? ShapeRecordContent { get; set; }
        public int ShapeRecordContentLength { get; set; }
        public double MinimumX { get; set; }
        public double MaximumX { get; set; }
        public double MinimumY { get; set; }
        public double MaximumY { get; set; }
    }

    public class BuildingExtractItemV2Configuration : IEntityTypeConfiguration<BuildingExtractItemV2>
    {
        private const string TableName = "BuildingV2";

        public void Configure(EntityTypeBuilder<BuildingExtractItemV2> builder)
        {
            builder.ToTable(TableName, Schema.Extract)
                .HasKey(p => p.PersistentLocalId)
                .IsClustered();

            builder.Property(p => p.PersistentLocalId)
                .ValueGeneratedNever();

            builder.Property(p => p.DbaseRecord);
            builder.Property(p => p.ShapeRecordContent);
            builder.Property(p => p.ShapeRecordContentLength);
            builder.Property(p => p.MaximumX);
            builder.Property(p => p.MinimumX);
            builder.Property(p => p.MinimumY);
            builder.Property(p => p.MaximumY);
        }
    }
}
