using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Command.Application;

namespace Product.Command.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ILogger<WeatherForecastController> _logger;
    public ProductController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
