using MassTransit;

namespace Waiter2.Contracts;

public class GrindBeansCommand : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}