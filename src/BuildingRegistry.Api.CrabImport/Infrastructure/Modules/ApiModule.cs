namespace BuildingRegistry.Api.CrabImport.Infrastructure.Modules
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.CommandHandling.Idempotency;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing.Autofac;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.EventHandling.Autofac;
    using Be.Vlaanderen.Basisregisters.GrAr.Import.Api;
    using Be.Vlaanderen.Basisregisters.GrAr.Import.Processing.CrabImport;
    using Be.Vlaanderen.Basisregisters.MessageHandling.AwsSqs.Simple;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore.Autofac;
    using BuildingRegistry.Infrastructure;
    using BuildingRegistry.Infrastructure.Modules;
    using CrabImport;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
using System;

    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly ILoggerFactory _loggerFactory;

        public ApiModule(
            IConfiguration configuration,
            IServiceCollection services,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _services = services;
            _loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var eventSerializerSettings = EventsJsonSerializerSettingsProvider.CreateSerializerSettings();

            builder
                .RegisterModule(new MediatRModule())

                .RegisterModule(new DataDogModule(_configuration))

                .RegisterModule(new IdempotencyModule(
                    _services,
                    _configuration.GetSection(IdempotencyConfiguration.Section).Get<IdempotencyConfiguration>().ConnectionString,
                    new IdempotencyMigrationsTableInfo(Schema.Import),
                    new IdempotencyTableInfo(Schema.Import),
                    _loggerFactory))

                .RegisterModule(new EventHandlingModule(typeof(DomainAssemblyMarker).Assembly, eventSerializerSettings))

                .RegisterModule(new EnvelopeModule())

                .RegisterModule(new CommandHandlingModule(_configuration))

                .RegisterModule(new CrabImportModule(
                    _services,
                    _configuration.GetConnectionString("CrabImport"),
                    Schema.Import,
                    _loggerFactory));

            builder
                .RegisterType<IdempotentCommandHandlerModule>()
                .AsSelf();

            builder
                .RegisterType<IdempotentCommandHandlerModuleProcessor>()
                .As<IIdempotentCommandHandlerModuleProcessor>();

            var projectionsConnectionString = _configuration.GetConnectionString("Sequences");

            _services
                .AddDbContext<SequenceContext>(options => options
                    .UseLoggerFactory(_loggerFactory)
                    .UseSqlServer(projectionsConnectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure();
                        sqlServerOptions.MigrationsHistoryTable(MigrationTables.Sequence, Schema.Sequence);
                    }));

            builder
                .RegisterType<SqlPersistentLocalIdGenerator>()
                .As<IPersistentLocalIdGenerator>();

            builder
                .RegisterType<ProblemDetailsHelper>()
                .AsSelf();

            // register SqsOptions instance
            var sqsOptions = new SqsOptions();
            builder
                .RegisterInstance(sqsOptions);

            builder.Populate(_services);
        }
    }
}
