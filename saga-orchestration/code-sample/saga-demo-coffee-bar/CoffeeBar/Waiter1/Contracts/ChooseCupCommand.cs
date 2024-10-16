using MassTransit;

namespace Waiter1.Contracts;

public class ChooseCupCommand : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}