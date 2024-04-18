using System.Reflection;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Product.Command.API;
using Product.Command.Application;
using Product.Command.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Database Service
//builder.Services.AddDbContext<MovieDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CleanMovie.API")));
builder.Services.AddDbContext<MssqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlDbConnectionString")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Registering the exception handling middleware 
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
