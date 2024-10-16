using MassTransit;
using Waiter4.Contracts;

namespace Waiter4.Services;

public class ServeConsumer(ILogger<ServeConsumer> logger) : IConsumer<ServeCommand>
{
    public async Task Consume(ConsumeContext<ServeCommand> context)
    {
        await Task.Delay(2000);
        await context.Publish(new ServeCompleted { CorrelationId = context.Message.CorrelationId });
    }
}