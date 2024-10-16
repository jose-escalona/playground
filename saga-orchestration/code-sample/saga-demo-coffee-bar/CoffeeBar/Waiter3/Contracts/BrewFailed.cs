using MassTransit;

namespace Waiter3.Contracts;

public class BrewFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
}