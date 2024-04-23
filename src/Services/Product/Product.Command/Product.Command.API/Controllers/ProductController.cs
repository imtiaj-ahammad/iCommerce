using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Command.Application;

namespace Product.Command.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private IMediator _mediator;
    private readonly CreateProductCommandValidator _createProductCommandValidator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    private readonly IBus _bus;
    private readonly ILogger<WeatherForecastController> _logger;
    public ProductController(ILogger<WeatherForecastController> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        _logger.LogInformation("Seri Log is Working");
        var validationResult = _createProductCommandValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.ToString());
        }
        //commented for developing masstransit-> return Ok(await Mediator.Send(command));
        //
        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
        // we are naming our queue as ticketQueue, if it does not exist, RabbitMQ will create one.
        var endPoint = await _bus.GetSendEndpoint(uri);
        await endPoint.Send(command);
        return Ok();
        //
    }
}
