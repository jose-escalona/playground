using MassTransit;
using Waiter1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit((x =>
{
    x.AddConsumer<ChooseCupConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
}));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();