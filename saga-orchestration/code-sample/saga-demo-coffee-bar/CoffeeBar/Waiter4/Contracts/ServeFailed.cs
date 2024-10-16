using MassTransit;

namespace Waiter4.Contracts;

public class ServeFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
}