using MassTransit;

namespace Waiter4.Contracts;

public class ServeCommand : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}