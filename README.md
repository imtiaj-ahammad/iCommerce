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
    dotnet sln add Services/Product/Product.Command.Persistence/Product.Command.Persistence
    
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
8. 