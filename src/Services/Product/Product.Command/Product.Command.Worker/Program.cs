using System.Reflection;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Command.Worker;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
