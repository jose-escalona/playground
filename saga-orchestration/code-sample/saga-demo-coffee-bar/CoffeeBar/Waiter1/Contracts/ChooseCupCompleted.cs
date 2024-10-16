using MassTransit;

namespace Waiter1.Contracts;

public class ChooseCupCompleted : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}