using MainWaiter.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderCoffeeStateMachine, OrderCoffeeState>()
        .InMemoryRepository();
    
    x.UsingRabbitMq((context, config) =>
    {
        config.ReceiveEndpoint("order-coffee", e =>
        {
            e.UseMessageRetry(r =>
            {
                r.Interval(5, TimeSpan.FromSeconds(5));
            });
            e.ConfigureSaga<OrderCoffeeState>(context);
        });
    });
    
});

builder.Services.AddLogging(opt =>
{
    opt.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[HH:mm:ss] ";
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();