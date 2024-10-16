using MassTransit;

namespace CoffeeBar.Contracts;

public class OrderCoffeeCommand : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
    public string Customer { get; set; }
}