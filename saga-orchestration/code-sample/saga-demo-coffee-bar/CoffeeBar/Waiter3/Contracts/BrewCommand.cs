using MassTransit;

namespace Waiter3.Contracts;

public class BrewCommand : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}