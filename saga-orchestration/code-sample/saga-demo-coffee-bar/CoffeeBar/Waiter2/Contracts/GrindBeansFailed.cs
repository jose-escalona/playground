using MassTransit;

namespace Waiter2.Contracts;

public class GrindBeansFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
}