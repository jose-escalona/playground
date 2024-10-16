using MassTransit;
using Waiter4;
using Waiter4.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((x =>
{
    x.AddConsumer<ServeConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
}));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();