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
#### domains,interfaces,implementations for Product.Query-start
30. Lets add the interfaces for Product.Query
    ```
    cd Product.Query.Application
    mkdir Abstractions
    cd Abstractions
    mkdir Repositories
    cd Repositories
    dotnet new interface -n IGenericRepository
    ```
    ```
    public interface IGenericRepository <T> where T : class
    {
	T GetById(int id);
	IEnumerable<T> GetAll();
	IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
    ```
    ```
    dotnet new interface -n IProductQueryRepository
    ```
    ```
    public interface IProductQueryRepository: IGenericRepository<Product.Query.Domain.Product>
    {

    }
    ```
    ```
    dotnet new interface -n IUnitOfWork
    ```
    ```
    public interface IUnitOfWork : IDisposable
    {
    IProductQueryRepository ProductQueryRepository { get; }
    Task<bool> SaveChangesAsync();
    }
    ```
    ```
    cd Product.Query
    dotnet add Product.Query.Application/Product.Query.Application.csproj reference  Product.Query.Domain/Product.Query.Domain.csproj
    ```
31. Let's go to Product.Query.Domain and add the domain class
    ```
    cd Product.Query.Domain
    mkdir Entities
    cd Entities
    dotnet new class -n EntityBase
    dotnet new class -n Product
    ```
    ```
    public abstract class EntityBase
    {
    public int Id { get; set; }
    /*public Guid Id { get; set; }
    public virtual Guid CreatedBy { get; set; }
    public virtual DateTime CreateDate { get; set; }
    public virtual DateTime LastUpdateDate { get; set; }
    public virtual Guid LastUpdatedBy { get; set; }
    public bool IsMarkedToDelete { get; set; }
    public virtual DateTime DeletedDate { get; set; }
    public virtual Guid DeletedBy { get; set; }
    public virtual string Remarks {get; set;}*/ 
    }
    ```
    ```
    public class Product : EntityBase
    {
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public Guid ExtraId { get; set; }
    }
    ```
    ```
    cd Product.Query.Persistence
    mkdir DbContext
    cd DbContext
    dotnet new class -n MongoDbContext
    cd..
    mkdir Repositories
    cd Repositories
    dotnet new class -n GenericRepository
    dotnet new class -n ProductQueryRepository
    dotnet new class -n UnitOfWork
    ```
    ```
    public class MobDbContext  : DbContext
    {
    public MobDbContext(DbContextOptions<MobDbContext> options) : base(options) { }

		public DbSet<Product.Query.Domain.Product> Products { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Actor>().HasData(
					new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris" }
					, new Actor { Id = 2, FirstName = "Jane", LastName = "Doe" }
					, new Actor { Id = 3, FirstName = "Van", LastName = "Damme" }
				);
        }*/
    }
    ```
    ```
    public class GenericQueryRepository<T> : IGenericRepository<T> where T : class
    {
    public readonly MobDbContext _context;
    public GenericQueryRepository(MobDbContext context)
    {
        _context = context;
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }
    }
    ```
    ```
    public class ProductQueryRepository : GenericRepository<Product.Query.Domain.Product>, IProductQueryRepository
    {   
    public ProductQueryRepository(MobDbContext context) : base(context)
    {

    }
    }
    ```
    ```
    public class UnitOfWork : IUnitOfWork
    {
    private readonly MobDbContext _context;
    public UnitOfWork(MobDbContext context)
    {
        this._context = context;
        ProductCategoryQueryRepository = new ProductQueryRepository(_context);
    }

    public IProductQueryRepository ProductQueryRepository { get; private set; }

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
    ```
    dotnet add Product.Query.Persistence/Product.Query.Persistence.csproj reference  Product.Query.Application/Product.Query.Application.csproj
    ```
32. Let's add the following packages for EF and add the configurations into Product.Query.Persistence
    ```
    dotnet add package Microsoft.EntityFrameworkCore -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.16
    dotnet add package Microsoft.EntityFrameworkCore.Tools -v 6.0.16
    ```
33. Let's add Persistence and Infrastructure reference into API
    ```
    dotnet add Product.Query.API/Product.Query.API.csproj reference  Product.Query.Persistence/Product.Query.Persistence.csproj
    dotnet add Product.Query.API/Product.Query.API.csproj reference  Product.Query.Infrastructure/Product.Query.Infrastructure.csproj
    ```
34. Let's add new controller for Product
    ```
    cd Product.Query.API
    cd controllers
    dotnet new class -n ProductController
    ```
    ```
    public class ProductController : ControllerBase
    {
    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }
    }
    ```
#### domains,interfaces,implementations for Product.Query-end
#### Let's Implement CQRS for Product.Query-start
35. Install the following packages into Product.Query.Application
    ```
    cd Product.Query.Application
    dotnet add package MediatR -v 12.2.0
    ```
36. Let's add commands and commandHandler into Product.Query.Application
    ```
    cd Product.Query.Application
    mkdir Commands
    cd Commands
    mkdir Product
    cd Product
    dotnet new class -n  GetProductsQuery
    dotnet new class -n  GetProductsQueryHandler
    dotnet new class -n  GetProductByIdQuery
    dotnet new class -n  GetProductByIdQueryHandler
    ```
    ```
    public class GetProductQuery : IRequest<IEnumerable<Product.Query.Domain.Product>>
    {

    }
    ```
    ```
    public class GetProductByIdQuery : IRequest<Product.Query.Domain.Product>
    {
    public int Id { get; set; }
    }
    ```
    ```
    public class GetProductsQueryHandler  : IRequestHandler<GetProductsQuery, IEnumerable<Product.Query.Domain.Product>>
    {
    private readonly IUnitOfWork _unitOfWork;
    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Product.Query.Domain.Product>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var productList =  _unitOfWork.ProductQueryRepository.GetAll();
        if(productList == null)
        {
            return null;
        }
        return productList;
    }
    }
    ```
    ```
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product.Query.Domain.Product>
    {
    private readonly IUnitOfWork _unitOfWork;
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Product.Query.Domain.Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = /*await*/ _unitOfWork.ProductQueryRepository.GetById(query.Id);
        if(product == null )
        {
            return null;
        }
        return product;
    }
    }
    ```
    ```
    public class ProductController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<ProductController> _logger;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetProductsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

    }
    ```
#### Let's Implement CQRS for Product.Query-end

#### Redis implementation-start
37. Add the redis packages for Persistence and Application
    ```
    cd Product.Query.Persistence
    dotnet add package StackExchange.Redis
    dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis -v 6.0.16
    cd Product.Query.Application
    dotnet add package StackExchange.Redis
    dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis -v 6.0.16
    ```
38. Go to Product.Query.Application and add the interface for caching contract
    ```
    mkdir Caching
    cd Caching
    dotent new interface -n ICacheService 
    ```
    ```
    public interface ICacheService
    {
    T GetData<T>(string key);
    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    object RemoveData(string key);
    }
    ```
39. Go to Product.Query.Persistence and add the implementation of the interface for caching contract
    ```
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        public CacheService(IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            _cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var exists = _cacheDb.KeyExists(key);
            if(exists)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value),expiryTime);
        }
    }
    ```
40. Inject the implementation for ICacheService into Product.Query.API
    ```
    builder.Services.AddScoped<ICacheService, CacheService>();
    ```
41. Let go to the GetProductsQueryHandler and update it with redis service
    ```
    public class GetProductsQueryHandler  : IRequestHandler<GetProductsQuery, IEnumerable<Product.Query.Domain.Product>>
    {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<Product.Query.Domain.Product>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //check cache data
        var cacheData = _cacheService.GetData<IEnumerable<Product.Query.Domain.Product>>("products");

        if(cacheData != null && cacheData.Count() > 0)
        {
            return (IEnumerable<Product.Query.Domain.Product>)cacheData;
        }
        cacheData = /*await*/ _unitOfWork.ProductQueryRepository.GetAll();

        //Set Expiry time
        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<IEnumerable<Product.Query.Domain.Product>>("products",cacheData, expiryTime);

        return (IEnumerable<Product.Query.Domain.Product>)cacheData;
    }
    }
    ```
42. Add the connectionString for Redis on API
    ```
    "ConnectionStrings": {
    "MssqlDbConnectionString": "Server=DESKTOP-TM16N21; Database=ProductDb; Trusted_Connection=True;"
    ,"RedisConnectionString": "localhost:6379"
    }
    ```
43. Setup the redis image in docker desktop
    ```
    docker run --name my-redis -p 6379:6379 -d redis
    docker ps
    ```
#### Redis implementation-end
#### Option pattern-start
44. Go to API and make a new folder for ApplicationOptions
    ```
    cd Product.Query.API
    mkdir Configurations
    cd Configurations
    dotnet new class -n ApplicationOptions
    ```
45. Inject the ApplicationOptions into Program.cs 
    ```
    cd Product.Query.API
    ```
    ```
    builder.Services
      .AddOptions<ApplicationOptions>()
      .Bind(builder.Configuration.GetSection(nameof(ApplicationOptions)));
    ```
46. Update the ProductController with ApplicationOptions
    ```
    public class ProductController : ControllerBase
    {
        private readonly ApplicationOptions _applicationOptions;
        public ProductController(IOptionsSnapshot<ApplicationOptions> applicationOptions)
        {
            _logger = logger;
            _applicationOptions = applicationOptions.Value;
        }
    }
    ```
#### Option Pattern-end
#### API Versioning - start
47. Let's go to Product.Query.API and create folders for v1 and v2
    ```
    cd Product.Query.API
    mkdir Versioning
    cd Versioning
    dotnet new class -n ConfigureSwaggerOptions
    ```
    ```
    public class ConfigureSwaggerOptions: IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configure each API discovered for Swagger Documentation
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        /// <summary>
        /// Configure Swagger Options. Inherited from the Interface
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Create information about the version of the API
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Information about the API</returns>
        private OpenApiInfo CreateVersionInfo(
                ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = ".NET Core (.NET 6) Web API",
                Version = desc.ApiVersion.ToString()
            };

            if (desc.IsDeprecated)
            {
                info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return info;
        }
    }
    ```
48. Add the following packages for Product.Query.API
    ```
    dotnet add package Microsoft.AspNetCore.Mvc.Versioning -v 5.0.0
    dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer -v 5.0.0
    ```
49. Lets make folders for v1 and v2 and make controller for v1 and v2 and configure the api-version configuration in controllers
    ```
    cd Product.Query.API
    cd Controllers
    mkdir V1
    mkdir V2
    ```
    Move the **ProductController** into V1 and fix the namespace and update the api-version config attributes
    ```
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
    ```
    Create **ProductController** in V2 and update configs for V2
    ```
    namespace Product.Query.API.Controllers.V2;

    //[ApiController]
    //[Route("[controller]")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
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

        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetProductsQuery()));
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }
        
    }
    ```
50. Go to program.cs and update it accordingly
    ```
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Product.Query.API;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services
    .AddOptions<ApplicationOptions>()
    .Bind(builder.Configuration.GetSection(nameof(ApplicationOptions)));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new MediaTypeApiVersionReader("version"),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader("x-api-version")
            );
            //options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    // Add ApiExplorer to discover versions
    builder.Services.AddVersionedApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    ```
#### API Versioning - end

#### Serilog - start
51.  Go to Product.Command.API and  Install the following packages
    ```
    dotnet add package Serilog.AspNetCore
    dotnet add package Serilog
    dotnet add package Serilog.Sinks.Console
    dotnet add package Serilog.Sinks.File
    dotnet add package Serilog.Sinks.MSSqlServer
    ```
52. Go to program file and inject serilog
    ```
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    builder.Host.UseSerilog();
    ```
53. Now lets configure settings in appSettings.json
    ```
  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "../logs/webapi-.log",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level:u3} {Username} {Message:lj}{Exception}{NewLine}"
        }
      },
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}"
        }
      },
      {
        "Name": "MSSqlServer",
        "Args": {
          "connectionString": "Server=DESKTOP-TM16N21; Database=MovieRentalDb; Trusted_Connection=True; TrustServerCertificate=true",
          "tableName": "Logs",
          "autoCreateSqlTable": true
        }
      }
    ]
  }
    ```
54. Add the following package
    ```
    dotnet add package Serilog.Sinks.MSSqlServer
    ```
11. Let's go to the ProductController and create some logs
    ```
    _logger.LogInformation("Seri Log is Working");
    ```
#### Serilog - end





### References:
- Option Pattern 
    - https://medium.com/checkout-com-techblog/the-options-pattern-simplified-8e0e03b41a71
- API Versioning 
    - https://vivasoftltd.com/api-versioning-in-asp-net-core/
    - https://medium.com/@seldah/managing-multiple-versions-of-your-api-with-net-and-swagger-47b4143e8bf5
    - https://github.com/saideldah/api-versioining-dot-net-6
- Serilog
    - https://www.c-sharpcorner.com/article/how-to-implement-serilog-in-asp-net-core-web-api/
    - https://www.c-sharpcorner.com/article/how-to-implementation-serilog-in-asp-net-core-5-0-application-with-database/
    - https://medium.com/@oevrensel/logging-in-asp-net-core-17efca14d953