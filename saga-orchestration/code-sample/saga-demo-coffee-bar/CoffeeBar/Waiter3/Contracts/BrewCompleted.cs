using MassTransit;

namespace Waiter3.Contracts;

public class BrewCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}