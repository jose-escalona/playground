using MassTransit;
using Waiter3.Contracts;
using Waiter4.Contracts;

namespace Waiter3.Services;

public class BrewConsumer : IConsumer<BrewCommand>
{
    public async Task Consume(ConsumeContext<BrewCommand> context)
    {
        await Task.Delay(2000);
        await context.Publish(new BrewCompleted { CorrelationId = context.Message.CorrelationId });
    }
}