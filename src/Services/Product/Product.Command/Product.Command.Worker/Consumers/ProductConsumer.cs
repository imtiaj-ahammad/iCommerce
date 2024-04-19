using MassTransit;
using MediatR;
using Product.Command.Application;

namespace Product.Command.Worker;

public class ProductConsumer : IConsumer<CreateProductCommand>
{
    private readonly IMediator _mediator;
    public ProductConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<CreateProductCommand> context)
    {
        var createProductCommand = context.Message;//we are extracting the actual message from the Context
        await _mediator.Send(createProductCommand);
        //Validate the Ticket Data
        //Store to Database
        //Notify the user via Email / SMS
    }
}
