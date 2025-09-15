using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OutboxDesignPattern.Consumers;
using OutboxDesignPattern.Data;
using OutboxDesignPattern.Jobs;
using OutboxDesignPattern.Jobs.OutboxPublisher;
using OutboxDesignPattern.Mapping;
using OutboxDesignPattern.Services.Outbox;
using OutboxDesignPattern.Services.User;
using Quartz;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionString = "Server=host.docker.internal,1433;Database=OutboxDemo;User Id=sa;Password=StrongPass!123;TrustServerCertificate=True;";

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("host.docker.internal", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("Outbox.Events.V1.UserCreated", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("Outbox.Events.V1:UserCreated", s => s.ExchangeType = "fanout");
                    e.ConfigureConsumer<UserCreatedConsumer>(context);
                });
            });
        });

        services.AddScoped<IOutboxService, OutboxService>();
        services.AddScoped<IOutboxPublisherService, OutboxPublisherService>();
        services.AddScoped<IUserService, UserService>();

        services.AddQuartz(q =>
        {
            var outboxProcessorKey = new JobKey("OutboxProcessorJob");
            q.AddJob<OutboxProcessorJob>(opts => opts.WithIdentity(outboxProcessorKey));
            q.AddTrigger(opts => opts.ForJob(outboxProcessorKey)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()));

            var randomAddRecordKey = new JobKey("RandomAddRecordToOutboxProcessorJob");
            q.AddJob<OutboxSeederJob>(opts => opts.WithIdentity(randomAddRecordKey));
            q.AddTrigger(opts => opts.ForJob(randomAddRecordKey)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
    })
    .Build();

await host.RunAsync();
