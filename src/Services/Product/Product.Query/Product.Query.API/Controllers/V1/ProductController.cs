using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Product.Query.Application;

namespace Product.Query.API.Controllers.V1;

//[ApiController]
//[Route("[controller]")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProductController : ControllerBase
{
    private IMediator _mediator;
    private readonly ILogger<ProductController> _logger;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ApplicationOptions _applicationOptions;

    public ProductController(ILogger<ProductController> logger, IOptionsSnapshot<ApplicationOptions> applicationOptions)
    {
        _logger = logger;
        _applicationOptions = applicationOptions.Value;
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetProductsQuery()));
    }

    [MapToApiVersion("1.0")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
    }
    
}