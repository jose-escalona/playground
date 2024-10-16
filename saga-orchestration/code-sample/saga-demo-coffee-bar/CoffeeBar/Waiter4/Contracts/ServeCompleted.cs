using MassTransit;

namespace Waiter4.Contracts;

public class ServeCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}
