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
    dotnet new classlib -f net6.0 -n Product.Command.Application
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command.Application/Product.Command.Application.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Domain
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command.Domain/Product.Command.Domain.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Infrastructure
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command.Infrastructure/Product.Command.Infrastructure.csproj

    cd Services
    cd Product
    dotnet new classlib -f net6.0 -n Product.Command.Persistence
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command.Persistence/Product.Command.Persistence.csproj
    
    cd Services
    cd Product
    dotnet new webapi -f net6.0 -n Product.Command.API
    cd ..
    cd ..
    dotnet sln add Services/Product/Product.Command.API/Product.Command.API.csproj
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
    cd Services/Product
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
    cd product
    dotnet add Product.Command.API/Product.Command.API.csproj reference  Product.Command.Infrastructure/Product.Command.Infrastructure.csproj
    dotnet add Product.Command.API/Product.Command.API.csproj reference  Product.Command.Persistence/Product.Command.Persistence.csproj
    dotnet add Product.Command.Persistence/Product.Command.Persistence.csproj reference  Product.Command.Application/Product.Command.Application.csproj
    ```
11. 


