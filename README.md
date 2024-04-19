#### iCommerce
### An eCommerce demo backend application with clean architecture including prime tools and libraries fulfilling a microservice architecture

1. Go to a directory destination and create a project folder and 
    ```
    mkdir iCommerce
    cd iCommerce
    ```
2. Create a gitignore file
    ```
    dotnet new gitignore
    ```
3. Create a README.md file
    ```
    touch README.md
    ```
4. Create a folder for sourceCodes and create a blank solution
    ```
    mkdir src
    cd src
    dotnet new sln -n iCommerce
    ```
5. Create a folder for services in src and add folders and projects accordingly-
    ```
    cd src
    mkdir Services
    cd Services
    mkdir Product
    cd Product
    mkdir Product.Command
    cd Product.Command
    dotnet new classlib -f net6.0 -n Product.Command.Application
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.Application/Product.Command.Application.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Domain
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.Domain/Product.Command.Domain.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Infrastructure
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.Infrastructure/Product.Command.Infrastructure.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Persistence
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.Persistence/Product.Command.Persistence.csproj
    
    cd Services
    cd Product
    dotnet new webapi -f net6.0 -n Product.Command.API
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.API/Product.Command.API.csproj
    ```
6. Adding EntityBase class 
    ```

    cd Product.Command.Domain
    mkdir Entities
    cd Entities
    dotnet new class -n EntityBase
    ```
    ```
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastUpdateDate { get; set; }
        public virtual Guid LastUpdatedBy { get; set; }
        public bool IsMarkedToDelete { get; set; }
        public virtual DateTime DeletedDate { get; set; }
        public virtual Guid DeletedBy { get; set; }
        public virtual string Remarks {get; set;}    
    }
    ```
7. Adding Product Entity
    ```
    public class Product : EntityBase
    {
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal? Price { get; private set; }
    }
    ```
8. Now let's add the interfaces for product
    ```
    cd Product.Command.Application
    mkdir Abstractions
    cd Abstractions
    mkdir Repositories
    cd Repositories
    dotnet new interface -n IGenericRepository
    ```
    ```
    public interface IGenericRepository<T> where T : class
    {
	void Add(T entity);
	void AddRange(IEnumerable<T> entities);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
    }
    ```
    ```
    cd Repositories
    dotnet new interface -n IProductCommandRepository
    ```
    ```
    public interface IProductCommandRepository: IGenericRepository<Product.Command.Domain.Product>
    {

    }
    ```
    ```
    cd Product.Command
    dotnet add Product.Command.Application/Product.Command.Application.csproj reference  Product.Command.Domain/Product.Command.Domain.csproj
    ```
    ```
    cd Repositories
    dotnet new interface -n IUnitOfWork
    ```
    ```
    public interface IUnitOfWork : IDisposable
    {
    IProductCommandRepository ProductCommandRepository { get; }
	int Save();
    }
    ```
9. We will add the implementation for product interfaces
    ```
    cd Product.Command.Persistence
    mkdir DbContext
    cd DbContext
    dotnet new class -n MssqlDbContext
    ```
    ```
    public class MssqlDbContext  : DbContext
    {
    public MssqlDbContext(DbContextOptions<MssqlDbContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }
    }
    ```
    ```
    cd Product.Command.Persistence
    mkdir Repositories
    cd Repositories
    dotnet new class -n GenericCommandRepository
    ```
    ```
    public class GenericCommandRepository<T> : IGenericCommandRepository<T> where T : class
    {
    public readonly MssqlDbContext _context;
    public GenericCommandRepository(MssqlDbContext context)
    {
         _context = context;
    }
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
    }
    ```
    ```
    cd Product.Command.Persistence/Repositories
    dotnet new class -n ProductCommandRepository
    ```
    ```
    public class ProductCommandRepository : GenericCommandRepository<Product>, IProductCommandRepository
    {
        public ProductCommandRepository(MssqlDbContext context) : base(context)
        {

        }
    }
    ```
    ```
    cd Product.Command.Persistence/Repositories
    dotnet new class -n UnitOfWork
    ```
    ```
    public class UnitOfWork : IUnitOfWork
    {
    private readonly MssqlDbContext _context;
    public UnitOfWork(MssqlDbContext context)
    {
        this._context = context;
        ProductCommandRepository = new ProductCommandRepository(_context);
    }

    public IProductCommandRepository ProductCommandRepository { get; private set; }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int Save()
    {
        return _context.SaveChanges();
    }
    }
    ```
10. let's add the following packages for sql and add the configurations into Product.Command.Persistence
    ```
    dotnet add package Microsoft.EntityFrameworkCore -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.16
    ```
    Add the db connectionString in Product.Command.API.appsettings.Development.json
    ```
    "ConnectionStrings": {
    "MssqlDbConnectionString": "Server=DESKTOP-TM16N21; Database=ProductDb; Trusted_Connection=True;"
    }
    ```
    Inject SqlServer with connectionString into Product.Command.API.program.cs
    ```
    builder.Services.AddDbContext<MssqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlDbConnectionString")));
    ```
    Add the following packages into Product.Command.API
    ```
    dotnet add package Microsoft.EntityFrameworkCore -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.16
    ```
    Add Persistence reference into Product.Command.API
    ```
    cd Product.Command/
    dotnet add Product.Command.API/Product.Command.API.csproj reference  Product.Command.Infrastructure/Product.Command.Infrastructure.csproj
    dotnet add Product.Command.API/Product.Command.API.csproj reference  Product.Command.Persistence/Product.Command.Persistence.csproj
    dotnet add Product.Command.Persistence/Product.Command.Persistence.csproj reference  Product.Command.Application/Product.Command.Application.csproj
    ```
#### CQRS implementation-start
11. Install the following packages into Product.Command.Application
    ```
    dotnet add package MediatR
    ```
12. Go to Application and create folder for commands
    ```
    cd Product.Command.Application
    mkdir Commands
    cd Commands
    mkdir Product
    cd Product
    dotnet new class -n CreateProductCommand
    ```
    ```
    public class CreateProductCommand  : IRequest<Guid>
    {
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    }
    ```
    ```
    dotnet new class -n CreateProductCommandHandler
    ```
    ```
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productObj = new Product.Command.Domain.Product();
            //productObj.Id = Guid.NewGuid();
            productObj.Name = command.Name;
            productObj.Description = command.Description;
            productObj.Price = command.Price;
            _unitOfWork.ProductCommandRepository.Add(productObj);
            await _unitOfWork.SaveChangesAsync();
            return productObj.Id;
        }
    }
    ```
    ```
    cd Product.Command.API/Controllers
    dotnet new class -n ProductController
    ```
    ```
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productObj = new Product.Command.Domain.Product();
            //productObj.Id = Guid.NewGuid();
            productObj.Name = command.Name;
            productObj.Description = command.Description;
            productObj.Price = command.Price;
            _unitOfWork.ProductCommandRepository.Add(productObj);
            await _unitOfWork.SaveChangesAsync();
            return productObj.Id;
        }
    }
    ```
#### CQRS implementation-end
#### FluentValidation-start
13. Install the following packages
    ```
    dotnet add package FluentValidation -v 11.9.0  
    //dotnet add package FluentValidation.AspNetCore -v 11.3.0  
    //dotnet add package FluentValidation.DependencyInjectionExtensions -v 11.9.0
    ```
14. Add a new folder for Validators in application
    ```
    mkdir Validators
    cd Validators
    mkdir Product
    cd Product
    dotnet new class -n CreateProductCommandValidator
    ```
    ```
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
    public CreateProductCommandValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(user => user.Price).GreaterThanOrEqualTo(0).WithMessage("Price must positive number.");
    }

    }
    ```
    ```
    public class ProductController : ControllerBase
    {
    private IMediator _mediator;
    private readonly CreateProductCommandValidator _createProductCommandValidator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private readonly ILogger<WeatherForecastController> _logger;
    public ProductController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var validationResult = _createProductCommandValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.ToString());
        }
        return Ok(await Mediator.Send(command));
    }
    }
    ```
    Inject the validator in the Product.Command.API.Program.cs
    ```
    builder.Services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
    ```
#### FluentValidation-end
#### AutoMapper-start
15. Go to Product.Command.Application and add the required packages for automapper
    ```
    dotnet add package AutoMapper -v 12.0.1
    dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection -v 12.0.1
    ```
16. Configure AutoMapper into services
    ``` 
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
    ```
17. Go to application and make folder for mapping profiles
    ```
    mkdir MapplingProfiles
    cd MappingProfiles
    mkdir Product
    cd Product
    dotnet new class -n ProductMappingProfile
    ```
    ```
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product.Command.Domain.Product>()
                    .ForMember(dest => dest.ExtraId,
                        opt =>
                            opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
    ```
    ```
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            var productObj0 = _mapper.Map<Product.Command.Domain.Product>(command);

            var productObj = new Product.Command.Domain.Product();
            //productObj.Id = Guid.NewGuid();
            productObj.Name = command.Name;
            productObj.Description = command.Description;
            productObj.Price = command.Price;
            _unitOfWork.ProductCommandRepository.Add(productObj);
            await _unitOfWork.SaveChangesAsync();
            return productObj.Id;
        }
    }
    ```
#### AutoMapper-end
#### GlobalExceptionHandler-start
18. Install the following packages
    ```
    cd Product.Command.API
    dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 6.0.28
    dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson -v 6.0.28
    ```
19. create a new folder and create class for exceptionMiddleware
    ```
    mkdir Middleware
    cd Middleware
    dotnet new class -n ExceptionMiddleware
    ```
    ```
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var errorResponse = new
                {
                    message = "An error occurred while processing your request.",
                    details = exception.Message
                };

                var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);

                return context.Response.WriteAsync(jsonErrorResponse);
        }
    }
    ```
20. Inject ExceptionMiddleware into program
    ```
    //Registering the exception handling middleware 
    app.UseMiddleware<ExceptionMiddleware>();
    ```
#### GlobalExceptionHandler-end

#### RabbitMQ MassTransit -start
21. Add the following packages
    ```
    dotnet add package MassTransit --version 6.3.2
    dotnet add package MassTransit.RabbitMQ --version 6.3.2
    dotnet add package MassTransit.AspNetCore --version 6.3.2
    ```
22. We will now configure the Product.Command.API as a publisher in ASP.NET Core container. Go to program file and add the followings-
    ```
    // Add masstransit
    builder.Services.AddMassTransit(x => 
    {
        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config => 
            {
                config.UseHealthCheck(provider);
                config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
            }));
    });// this add the MassTransit Service to the ASP.NET Core service container
    builder.Services.AddMassTransitHostedService();// this creates a new Service Bus using RabbitMQ with the provided paremeters like the host url, username, password etc
    ```
23. Update the product controller for masstransit
    ```
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
    ```
24. Let's create a webapi application that will consume all the requests from queue
    ```
    cd src
    mkdir Services
    cd Services
    mkdir Product
    cd Product
    mkdir Product.Command
    cd Product.Command
    dotnet new webapi -f net6.0 -n Product.Command.Worker
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command/Product.Command.Worker/Product.Command.Worker.csproj
    ```
25. Add the following packages
    ```
    dotnet add package MassTransit --version 6.3.2
    dotnet add package MassTransit.RabbitMQ --version 6.3.2
    dotnet add package MassTransit.AspNetCore --version 6.3.2
    ```
26. Add Application reference to the Worker
    ```
    dotnet add Product.Command.Worker/Product.Command.Worker.csproj reference  Product.Command.Application/Product.Command.Application.csproj
    ```
27. Create a folder named Consumers and create a class named ProductConsumer
    ```
    mkdir Consumers
    cd Consumers
    dotnet new class -n ProductConsumer
    ```
    ```
    ```
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
    ```
28. We will now configure the Product.Command.Worker as a consumer in ASP.NET Core container. Go to program file and add the followings-
    ```
    // Add services to the container.
    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    // Add masstransit  
    builder.Services.AddMassTransit(x => 
        {
            x.AddConsumer<ProductConsumer>();
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config => 
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                    config.ReceiveEndpoint("ticketQueue", ep =>
                        {
                            ep.PrefetchCount = 16;
                            ep.UseMessageRetry(rt => rt.Interval(2, 100));
                            ep.ConfigureConsumer<ProductConsumer>(provider);
                        });
                }));
        });
    builder.Services.AddMassTransitHostedService();
    ```
#### RabbitMQ MassTransit -start
#### Product.Query-structure-start
29. Let's build the structure for Product.Query
    ```
    cd Product
    mkdir Product.Query
    cd Product.Query

    dotnet new classlib -f net6.0 -n Product.Query.Application
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Query/Product.Query.Application/Product.Query.Application.csproj

    cd Product.Query/
    dotnet new classlib -f net6.0 -n Product.Query.Domain
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Query/Product.Query.Domain/Product.Query.Domain.csproj

    cd Services
    cd Product.Query/
    dotnet new classlib -f net6.0 -n Product.Query.Infrastructure
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Query/Product.Query.Infrastructure/Product.Query.Infrastructure.csproj

    cd Services
    cd Product.Query/
    dotnet new classlib -f net6.0 -n Product.Query.Persistence
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Query/Product.Query.Persistence/Product.Query.Persistence.csproj
    
    cd Services
    cd Product.Query/
    dotnet new webapi -f net6.0 -n Product.Query.API
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Query/Product.Query.API/Product.Query.API.csproj
    ```
#### Product.Query-structure-end
30. 