using MassTransit;

namespace MainWaiter.Services;

public class OrderCoffeeState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int ReadyToBrewStatus { get; set; }
    public DateTime StartTime { get; set; }
}