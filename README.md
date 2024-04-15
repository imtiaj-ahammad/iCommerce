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
5. Create class libraries and add to the solution for followings-
    ```
    dotnet new classlib -f net6.0 -n eCommerce.Domain
    dotnet sln add eCommerce.Domain/eCommerce.Domain.csproj
    dotnet new classlib -f net6.0 -n eCommerce.Application
    dotnet sln add eCommerce.Application/eCommerce.Application.csproj
    dotnet new classlib -f net6.0 -n eCommerce.Infrastructure
    dotnet sln add eCommerce.Infrastructure/eCommerce.Infrastructure.csproj
    dotnet new classlib -f net6.0 -n eCommerce.Persistence
    dotnet sln add eCommerce.Persistence/eCommerce.Persistence.csproj
    ```
6. Now lets create a separate folder for presentation layer projects
    ```
    mkdir eCommerce.Presentation
    cd eCommerce.Presentation
    mkdir Services
    cd Services
    mkdir Product
    cd Product
    dotnet new webapi -f net6.0 -n Product.Command.API
    dotnet sln add eCommerce.Presentation/Services/Product/Product.Command.API/Product.Command.API.csproj

    
    dotnet new webapi -f net6.0 -n Product.Query.API --may be minimal API + nosql
    dotnet sln add eCommerce.Presentation/Services/Product/Product.Query.API/Product.Query.API.csproj
    ```
7. Let's go to Domain and create base class **EntityBase**
    ```
    cd ecommerce.Domain
    mkdir Models
    cd Models
    mkdir BaseModels
    cd BaseModels
    dotnet new class -n EntityBase
    ```
    ```
    public abstract class EntityBase
    {
        public Guid ItemId { get; set; }
        public virtual Guid CreatedBy { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime LastUpdateDate { get; set; }
        public virtual Guid LastUpdatedBy { get; set; }
        public bool IsMarkedToDelete { get; set; }
        public virtual DateTime DeletedDate { get; set; }
        public virtual Guid DeletedBy { get; set; }
    }
    ```
8. Let's make **ProductCategory** class 
    ```
    cd Models
    mkdir Product
    cd Product
    dotnet new class -n ProductCategory
    ```
    ```
    public class ProductCategory : EntityBase
    {
        public string Name { get; set;}   
    }
    ```
9. 