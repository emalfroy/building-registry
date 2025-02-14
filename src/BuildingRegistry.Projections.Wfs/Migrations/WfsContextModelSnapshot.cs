﻿// <auto-generated />
using System;
using BuildingRegistry.Projections.Wfs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace BuildingRegistry.Projections.Wfs.Migrations
{
    [DbContext(typeof(WfsContext))]
    partial class WfsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DesiredState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Name"));

                    b.ToTable("ProjectionStates", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.Building.Building", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Geometry>("Geometry")
                        .HasColumnType("sys.geometry");

                    b.Property<string>("GeometryMethod")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VersionAsString")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("BuildingId"));

                    b.HasIndex("GeometryMethod");

                    b.HasIndex("Id");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Status");

                    b.HasIndex("VersionAsString");

                    b.HasIndex("IsComplete", "IsRemoved");

                    b.ToTable("Buildings", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.BuildingUnit.BuildingUnit", b =>
                {
                    b.Property<Guid>("BuildingUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("BuildingUnitPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Function")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsBuildingComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<Geometry>("Position")
                        .HasColumnType("sys.geometry");

                    b.Property<string>("PositionMethod")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VersionAsString")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingUnitId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("BuildingUnitId"));

                    b.HasIndex("BuildingId");

                    b.HasIndex("BuildingPersistentLocalId");

                    b.HasIndex("BuildingUnitPersistentLocalId");

                    b.HasIndex("Function");

                    b.HasIndex("Id");

                    b.HasIndex("PositionMethod");

                    b.HasIndex("Status");

                    b.HasIndex("VersionAsString");

                    b.HasIndex("IsComplete", "IsRemoved", "IsBuildingComplete");

                    b.ToTable("BuildingUnits", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.BuildingUnit.BuildingUnitBuildingItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("BuildingRetiredStatus")
                        .HasColumnType("int");

                    b.Property<bool?>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.HasKey("BuildingId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("BuildingId"), false);

                    b.HasIndex("BuildingPersistentLocalId");

                    b.ToTable("BuildingUnit_Buildings", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.BuildingUnitV2.BuildingUnitBuildingItemV2", b =>
                {
                    b.Property<int>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("BuildingRetiredStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.HasKey("BuildingPersistentLocalId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("BuildingPersistentLocalId"), false);

                    b.HasIndex("BuildingPersistentLocalId");

                    b.ToTable("BuildingUnit_BuildingsV2", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.BuildingUnitV2.BuildingUnitV2", b =>
                {
                    b.Property<int>("BuildingUnitPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Function")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<Geometry>("Position")
                        .HasColumnType("sys.geometry");

                    b.Property<string>("PositionMethod")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VersionAsString")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingUnitPersistentLocalId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("BuildingUnitPersistentLocalId"));

                    b.HasIndex("BuildingPersistentLocalId");

                    b.HasIndex("BuildingUnitPersistentLocalId");

                    b.HasIndex("Function");

                    b.HasIndex("Id");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("PositionMethod");

                    b.HasIndex("Status");

                    b.HasIndex("VersionAsString");

                    b.ToTable("BuildingUnitsV2", "wfs");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Wfs.BuildingV2.BuildingV2", b =>
                {
                    b.Property<int>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<Geometry>("Geometry")
                        .HasColumnType("sys.geometry");

                    b.Property<string>("GeometryMethod")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VersionAsString")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("PersistentLocalId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("PersistentLocalId"));

                    b.HasIndex("GeometryMethod");

                    b.HasIndex("Id");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Status");

                    b.HasIndex("VersionAsString");

                    b.ToTable("BuildingsV2", "wfs");
                });
#pragma warning restore 612, 618
        }
    }
}
