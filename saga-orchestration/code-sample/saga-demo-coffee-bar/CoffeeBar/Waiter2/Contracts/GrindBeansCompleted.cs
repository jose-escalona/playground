using MassTransit;

namespace Waiter2.Contracts;

public class GrindBeansCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}