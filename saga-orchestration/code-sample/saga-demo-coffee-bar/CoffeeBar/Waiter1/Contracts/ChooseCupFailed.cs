using MassTransit;

namespace Waiter1.Contracts;

public class ChooseCupFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
}