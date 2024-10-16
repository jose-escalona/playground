using MassTransit;
using Waiter1.Contracts;
using Waiter2.Contracts;

namespace Waiter1.Services;

public class ChooseCupConsumer : IConsumer<ChooseCupCommand>
{
    public async Task Consume(ConsumeContext<ChooseCupCommand> context)
    {
        await Task.Delay(5000);
        await context.Publish(new ChooseCupCompleted { CorrelationId = context.Message.CorrelationId });
    }
}