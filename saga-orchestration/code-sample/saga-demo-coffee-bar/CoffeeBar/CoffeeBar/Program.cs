using CoffeeBar.Contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit((x =>
{
    x.UsingRabbitMq((_, _) => { });
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/coffee", (IBus bus) =>
    {
        bus.Publish<OrderCoffeeCommand>(new() { CorrelationId = Guid.NewGuid(), Customer= "John Doe" });
    })
    .WithOpenApi();

app.Run();