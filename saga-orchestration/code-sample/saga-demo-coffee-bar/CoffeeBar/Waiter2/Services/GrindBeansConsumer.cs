using MassTransit;
using Waiter2.Contracts;
using Waiter3.Contracts;

namespace Waiter2.Services;

public class GrindBeansConsumer : IConsumer<GrindBeansCommand>
{
    public async Task Consume(ConsumeContext<GrindBeansCommand> context)
    {
        await Task.Delay(5000);
        await context.Publish(new GrindBeansCompleted { CorrelationId = context.Message.CorrelationId });
    }
}