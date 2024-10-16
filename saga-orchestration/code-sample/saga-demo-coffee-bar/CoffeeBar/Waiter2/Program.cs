using MassTransit;
using Waiter2;
using Waiter2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((x =>
{
    x.AddConsumer<GrindBeansConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
}));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();