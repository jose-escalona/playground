using CoffeeBar.Contracts;
using MassTransit;
using Waiter1;
using Waiter1.Contracts;
using Waiter2;
using Waiter2.Contracts;
using Waiter3;
using Waiter3.Contracts;
using Waiter4;
using Waiter4.Contracts;

namespace MainWaiter.Services;

public class OrderCoffeeStateMachine : MassTransitStateMachine<OrderCoffeeState>
{
    private readonly ILogger<OrderCoffeeStateMachine> _logger;
    
    public State? CoffeOrdered { get; private set; }
    public State? CupChosen { get; private set; }
    public State? BeansGrinded { get; private set; }
    public State? Brewed { get; private set; }
    
    public Event<OrderCoffeeCommand>? OrderCoffeeEvent { get; set; }
    public Event<ChooseCupCompleted>? ChooseCupCompletedEvent { get; set; }
    public Event<ChooseCupFailed>? ChooseCupFailedEvent { get; set; }
    public Event<GrindBeansCompleted>? GrindBeansCompletedEvent { get; set; }
    public Event<GrindBeansFailed>? GrindBeansFailedEvent { get; set; }
    public Event<BrewCompleted>? BrewCompletedEvent { get; set; }
    public Event<BrewFailed>? BrewFailedEvent { get; set; }
    public Event<ServeCompleted>? ServeCompletedEvent { get; set; }
    public Event<ServeFailed>? ServeFailedEvent { get; set; }
    
    public Event ReadyToBrewEvent { get; set; }

    public OrderCoffeeStateMachine(ILogger<OrderCoffeeStateMachine> logger)
    {
        _logger = logger;
            
        InstanceState(x => x.CurrentState);
        DefineEvents();
        // ConfigureSataStateMachine();
        ConfigureSataStateMachineWithCompositeEvent();
        
        SetCompletedWhenFinalized();
    }

    private void DefineEvents()
    {
        Event(() => OrderCoffeeEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => ChooseCupCompletedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => ChooseCupFailedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => GrindBeansCompletedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => GrindBeansFailedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => BrewCompletedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => BrewFailedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => ServeCompletedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
        Event(() => ServeFailedEvent, x => { x.CorrelateById(y => y.Message.CorrelationId); });
    }

    private void ConfigureSataStateMachine()
    {

        Initially(
            When(OrderCoffeeEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffe saga: {context.Saga.CorrelationId} - Request to order a coffee initialized."))
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.StartTime = DateTime.UtcNow;
                })
                .ThenAsync(async context =>
                {
                    await context.Publish(new ChooseCupCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(CoffeOrdered));
        
        During(CoffeOrdered,
            When(ChooseCupCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Choose cup has been completed."))
                .ThenAsync(async context =>
                {
                    await context.Publish(new GrindBeansCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(CupChosen),
            When(ChooseCupFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Choose cup has failed."))
                .Finalize());

        During(CupChosen,
            When(GrindBeansCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Grind beans has been completed."))
                .ThenAsync(async context =>
                {
                    await context.Publish(new BrewCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(BeansGrinded),
            When(GrindBeansFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Grind beans has failed."))
                .Finalize());
        
        During(BeansGrinded,
            When(BrewCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Brew has been completed."))
                .ThenAsync(async context =>
                {
                    await context.Publish(new ServeCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(Brewed),
            When(BrewFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Brew has failed."))
                .Finalize());
        
        During(Brewed,
            When(ServeCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Serve has been completed. Time elapsed: {(DateTime.UtcNow - context.Saga.StartTime).TotalSeconds}s."))
                .Finalize(),
            When(ServeFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Serve has failed."))
                .Finalize());
    }
    
    private void ConfigureSataStateMachineWithCompositeEvent()
    {

        Initially(
            When(OrderCoffeeEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffe saga: {context.Saga.CorrelationId} - Request to order a coffee initialized."))
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.StartTime = DateTime.UtcNow;
                })
                .ThenAsync(async context =>
                {
                    await context.Publish(new ChooseCupCommand {CorrelationId = context.Saga.CorrelationId});
                    await context.Publish(new GrindBeansCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(CoffeOrdered));
        
        During(CoffeOrdered,
            When(ChooseCupCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Choose cup has been completed.")),
            When(ChooseCupFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Choose cup has failed."))
                .Finalize(),
        When(GrindBeansCompletedEvent)
            .Then(context =>
                _logger.LogInformation(
                    $"Order coffee saga: {context.Saga.CorrelationId} - Grind beans has been completed.")),
        When(GrindBeansFailedEvent)
            .Then(context =>
                _logger.LogInformation(
                    $"Order coffee saga: {context.Saga.CorrelationId} - Grind beans has failed."))
            .Finalize());

        CompositeEvent(
            () => ReadyToBrewEvent,
            x => x.ReadyToBrewStatus,
            ChooseCupCompletedEvent,
            GrindBeansCompletedEvent);

        DuringAny(
            When(ReadyToBrewEvent)
                .ThenAsync(async context =>
                {
                    await context.Publish(new BrewCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(BeansGrinded));
        
        During(BeansGrinded,
            When(BrewCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Brew has been completed."))
                .ThenAsync(async context =>
                {
                    await context.Publish(new ServeCommand {CorrelationId = context.Saga.CorrelationId});
                })
                .TransitionTo(Brewed),
            When(BrewFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Brew has failed."))
                .Finalize());
        
        During(Brewed,
            When(ServeCompletedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Serve has been completed. Time elapsed: {(DateTime.UtcNow - context.Saga.StartTime).TotalSeconds}s."))
                .Finalize(),
            When(ServeFailedEvent)
                .Then(context =>
                    _logger.LogInformation(
                        $"Order coffee saga: {context.Saga.CorrelationId} - Serve has failed."))
                .Finalize());
    }
}